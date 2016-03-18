// *******************************************************************************
//  filename    :   linearActuatorHorizontalPortK.c
//  purpose     :   provide basic functionality for the horizontal linear actuator
//                  using the GPIO pins along the upper nibble of PORTK
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016/03/15
// *******************************************************************************


#include "timer.h"
#include "linearActuatorHorizontalPortK.h"


// inverse of horizontal actuator bits
static const unsigned char HORIZONTAL_ACTUATOR_MASK = 0xCF;

// signal to send to actuator circuit to enable current
static const unsigned char HORIZONTAL_ACTUATOR_ON_FLAG = 0x20;

// signal to send to actuator circuit to enable retraction
static const unsigned char HORIZONTAL_ACTUATOR_RETRACTING_FLAG = 0x10;

// amount of time in milliseconds to extend 4 inches (experiment with this)
static const unsigned long long int EXTENSION_INTERVAL_MS = 22000;


// function (HorizontalActuatorK*) -> void
// initializes PORTK and state of actuator to be fully retracted and off (make sure this reflects real life)
void horizontal_actuator_init(HorizontalActuatorK* a) {
    a->isOn = 0;
    a->isRetracted = 0;
    a->isRetracting = HORIZONTAL_ACTUATOR_RETRACTING_FLAG;
    DDRK |= ~(HORIZONTAL_ACTUATOR_MASK);
    horizontal_actuator_sync(a);
}

// function (HorizontalActuatorK*) -> void
// synchronizes PORTK with current state of struct
void horizontal_actuator_sync(HorizontalActuatorK* a) {

    unsigned char currentState = PORTK;             // get copy of PORTK contents
    currentState &= HORIZONTAL_ACTUATOR_MASK;       // remove bits 5, 4
    currentState |= a->isOn | a->isRetracting;      // send in state of isOn/isExtending to bits 5, 4
    PORTK = currentState;                           // update PORTK
}

// function (HorizontalActuatorK*) -> void
// turns off actuator
void horizontal_actuator_disable(HorizontalActuatorK* a) {
    a->isOn = 0;
    horizontal_actuator_sync(a);
}

// function (HorizontalActuatorK*) -> void
// fully retracts actuator if necessary
void horizontal_actuator_retract(HorizontalActuatorK* a) {

    if (a->isRetracted)
        return;

    a->isOn = HORIZONTAL_ACTUATOR_ON_FLAG;
    a->isRetracting = HORIZONTAL_ACTUATOR_RETRACTING_FLAG;
    horizontal_actuator_sync(a);
    timer_delay_ms(EXTENSION_INTERVAL_MS);
    horizontal_actuator_disable(a);
    a->isRetracted = 1;
}

// function (HorizontalActuatorK*) -> void
// fully extends actuator if necessary
void horizontal_actuator_extend(HorizontalActuatorK* a) {

    if (!a->isRetracted)
        return;

    a->isOn = HORIZONTAL_ACTUATOR_ON_FLAG;
    a->isRetracting = 0;
    horizontal_actuator_sync(a);
    timer_delay_ms(EXTENSION_INTERVAL_MS);
    horizontal_actuator_disable(a);
    a->isRetracted = 0;
}