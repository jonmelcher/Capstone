// *****************************************************************************
//  filename    :   linearActuatorVerticalPortA.c
//  purpose     :   provide basic functionality for the vertical linear actuator
//                  using the GPIO pins along the upper nibble of PORTK
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016/03/15
// *****************************************************************************


#include "timer.h"
#include "linearActuatorVerticalPortA.h"


// inverse of actuator bits
static const unsigned char VERTICAL_ACTUATOR_MASK = 0xF3;

// signal to send to actuator circuit to enable current
static const unsigned char VERTICAL_ACTUATOR_ON_FLAG = 0x08;

// signal to send to actuator circuit to enable retraction
static const unsigned char VERTICAL_ACTUATOR_RETRACTING_FLAG = 0x04;

// corresponds to a 2 1/2 inch actuation (NEED TO FIND THIS VALUE)
static const unsigned long long int DROP_INTERVAL_MS = 12500;

// corresponds to a 3 3/4 inch actuation (NEED TO FIND THIS VALUE)
static const unsigned long long int TIER_INTERVAL_MS = 18750;

// function (VerticalActuatorA*, unsigned char) -> void
// initializes PORTA and state of actuator to be fully retracted and off (make sure this reflects real life)
void vertical_actuator_init(VerticalActuatorA* a, unsigned char maxTier) {
    a->maxTier = maxTier;
    a->tier = 0;                // assume linear actuator is fully retracted
    a->isDropped = 1;           // meaning we are on bottom tier and in dropped position
    a->isOn = 0;
    a->isRetracting = VERTICAL_ACTUATOR_RETRACTING_FLAG;
    DDRA |= (VERTICAL_ACTUATOR_ON_FLAG | VERTICAL_ACTUATOR_RETRACTING_FLAG);
    vertical_actuator_sync(a);
}

// function (VerticalActuatorA*) -> void
// synchronizes PORTA with current state of struct
void vertical_actuator_sync(VerticalActuatorA* a) {

    unsigned char currentState = PORTA;          // get copy of PORTA contents
    currentState &= VERTICAL_ACTUATOR_MASK;      // remove bits 3, 2
    currentState |= a->isOn | a->isRetracting;    // send in state of isOn/isRetracting to bits 3, 2
    PORTA = currentState;                        // update PORTA
}

// function (VerticalActuatorA*) -> void
// turns off actuator
void vertical_actuator_disable(VerticalActuatorA* a) {
    a->isOn = 0;
    vertical_actuator_sync(a);
}

// function (VerticalActuatorA*) -> void
// actuates into bottom of cell and brings to OFF position
void vertical_actuator_drop(VerticalActuatorA* a) {

    if (a->isDropped)
        return;
    vertical_actuator_actuate(a, VERTICAL_ACTUATOR_RETRACTING_FLAG, DROP_INTERVAL_MS);
    a->isDropped = 1;
}

// function (VerticalActuatorA*) -> void
// actuates into center of cell and brings to OFF position
void vertical_actuator_lift(VerticalActuatorA* a) {
    if (!a->isDropped)
        return;
    vertical_actuator_actuate(a, 0, DROP_INTERVAL_MS);
    a->isDropped = 0;
}

// function (VerticalActuatorA*, unsigned char, unsigned long long int) -> void
// actuates in the desired direction for the desired amount of milliseconds then brings to an OFF position
void vertical_actuator_actuate(VerticalActuatorA* a, unsigned char extensionFlag, unsigned long long int ms) {
    a->isOn = VERTICAL_ACTUATOR_ON_FLAG;
    a->isRetracting = extensionFlag;
    vertical_actuator_sync(a);
    timer_delay_ms(ms);
    vertical_actuator_disable(a);
}

// function (VerticalActuatorA*, unsigned char) -> void
// actuates to next tier and brings to OFF position - will be in center of tier at end
void vertical_actuator_transition_tier(VerticalActuatorA* a, unsigned char nextTier) {

    unsigned char retraction = (nextTier < a->tier) ? VERTICAL_ACTUATOR_RETRACTING_FLAG : 0;
    unsigned char tierDifference = retraction ? a->tier - nextTier : nextTier - a->tier;

    if (!tierDifference || nextTier >= a->maxTier)
        return;
    if (a->isDropped)
    {
        vertical_actuator_lift(a);
        timer_delay_ms(1000);
    }

    vertical_actuator_actuate(a, retraction, TIER_INTERVAL_MS * tierDifference);
    a->tier = nextTier;
}

void vertical_actuator_home(VerticalActuatorA* a) {
    vertical_actuator_actuate(a, VERTICAL_ACTUATOR_RETRACTING_FLAG, TIER_INTERVAL_MS * 4);
}
