// ************************************************************************
//  filename    :   linearActuatorPortK.c
//  purpose     :   provide basic functionality for a linear actuator using
//                  the GPIO pins along the upper nibble of PORTK
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016/03/15
// ************************************************************************

#include "linearActuatorPortK.h"

//               HHLL
// K REGISTER -> 7654____

void actuator_init(ActuatorK* a, unsigned char isHighBits, unsigned char maxStates, unsigned long int stateIntervalMs) {

    a->state = 0;
    a->maxStates = maxStates;
    a->stateIntervalMs = stateIntervalMs;
    a->isHighBits = isHighBits;
    a->isOn = 0;
    a->isExtending = 0;
    if (a->isHighBits)
        DDRK |= 0xC0;
    else
        DDRK |= 0x30;

    actuator_sync(a);
}

void actuator_sync(ActuatorK* a) {
    if (a->isHighBits) {
        if (a->isOn)
            PORTK |= 0x80;
        else
            PORTK &= 0x7F;
        if (a->isExtending)
            PORTK |= 0x40;
        else
            PORTK &= 0xBF;
    }
    else {
        if (a->isOn)
            PORTK |= 0x20;
        else
            PORTK &= 0xDF;
        if (a->isExtending)
            PORTK |= 0x10;
        else
            PORTK &= 0xEF;
    }
}

void actuator_set_power(ActuatorK* a, unsigned char power) {
    a->isOn = power;
    actuator_sync(a);
}

void actuator_actuate(ActuatorK* a, unsigned char direction, unsigned long int ms) {

    // turn off actuator
    a->isOn = 0;
    actuator_sync(a);

    // set appropriate state and sync (starts actuating)
    a->isOn = 1;
    a->isExtending = direction;
    actuator_sync(a);

    // wait appropriate amount of time
    delay_ms(ms);

    // turn off actuator
    a->isOn = 0;
    actuator_sync(a);
}

void actuator_state_transition(ActuatorK* a, unsigned char state) {

    if (state == a->state || state > a->maxStates)
        return;

    unsigned char direction = (state > a->state) ? 1 : 0;
    unsigned char stateDifference = (state > a->state) ? state - a->state : a->state - state;

    actuator_actuate(a, direction, a->stateIntervalMs * stateDifference);
    a->state = state;
}