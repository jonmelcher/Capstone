// *****************************************************************************
//  filename    :   linearActuatorVerticalPortA.c
//  purpose     :   provide basic functionality for the vertical linear actuator
//                  using the GPIO pins along the upper nibble of PORTK
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016/03/15
// *****************************************************************************


#include "timer.h"
#include "linearActuatorVerticalPortA.h"


// private prototypes
void vertical_actuator_sync(VerticalActuatorA* a);
void vertical_actuator_disable(VerticalActuatorA* a);


// PIN 60 - POWER (ON / OFF)
// PIN 59 - DIRECTION (EXTENDING / RETRACTING)
// inverse of actuator bits
static const unsigned char VERTICAL_ACTUATOR_MASK = 0xF3;

// signal to send to actuator circuit to enable current
static const unsigned char VERTICAL_ACTUATOR_ON_FLAG = 0x08;

// signal to send to actuator circuit to enable retraction
static const unsigned char VERTICAL_ACTUATOR_EXTENDING_FLAG = 0x04;

// corresponds to a 2 1/2 inch actuation (12.5 seconds * 0.2 inches / second = 2.5 inches)
static const unsigned long long int DROP_INTERVAL_MS = 7500;

static const unsigned char TIERS = 3;

// corresponds to a 3 3/4 inch actuation (18.75 seconds * 0.2 inches / second = 3.75 inches)
static const unsigned long long int TIER_INTERVAL_UP_MS = 18500;
static const unsigned long long int TIER_INTERVAL_DOWN_MS = 18000;

// amount of time in milliseconds to delay after disabling actuator
static const unsigned long long int STOP_DELAY_MS = 100;

// initializes DDRA / PORTA and state of actuator to be fully retracted and off (make sure this reflects real life)
void vertical_actuator_init(VerticalActuatorA* a) {
    a->maxTier = TIERS;
    a->tier = 0;                                    // assume linear actuator is fully retracted
    a->isDropped = 0;                               // meaning we are on bottom tier and in dropped position
    a->isOn = 0;
    a->isExtending = 0;
    DDRA |= ~(VERTICAL_ACTUATOR_MASK);
    vertical_actuator_sync(a);
}

// synchronizes PORTA with current state of struct
void vertical_actuator_sync(VerticalActuatorA* a) {
    unsigned char currentState = PORTA;             // get copy of PORTA contents
    currentState &= VERTICAL_ACTUATOR_MASK;         // remove bits 3, 2
    currentState |= a->isOn | a->isExtending;       // send in state of isOn/isExtending to bits 3, 2
    PORTA = currentState;                           // update PORTA
}

// turns off actuator
void vertical_actuator_disable(VerticalActuatorA* a) {
    a->isOn = 0;
    vertical_actuator_sync(a);
    timer_delay_ms(STOP_DELAY_MS);
}

// actuates into bottom of cell and brings to OFF position
void vertical_actuator_drop(VerticalActuatorA* a) {
    if (a->isDropped)
        return;
    vertical_actuator_actuate(a, 0, DROP_INTERVAL_MS);
    a->isDropped = 1;
}

// actuates into center of cell and brings to OFF position
void vertical_actuator_lift(VerticalActuatorA* a) {
    if (!a->isDropped)
        return;
    vertical_actuator_actuate(a, VERTICAL_ACTUATOR_EXTENDING_FLAG, DROP_INTERVAL_MS);
    a->isDropped = 0;
}

// actuates in the desired direction for the desired amount of milliseconds then brings to an OFF position
void vertical_actuator_actuate(VerticalActuatorA* a, unsigned char actuationFlag, unsigned long long int ms) {

    // set up actuation state of actuator and synchronize
    a->isOn = VERTICAL_ACTUATOR_ON_FLAG;
    a->isExtending = actuationFlag;
    vertical_actuator_sync(a);

    // actuate
    timer_delay_ms(ms);

    // stop
    vertical_actuator_disable(a);
}

// actuates to next tier and brings to OFF position - will be in center of tier at end
void vertical_actuator_transition_tier(VerticalActuatorA* a, unsigned char nextTier) {

    // determine if target tier is above or below current tier in order to determine direction
    unsigned char extension = (nextTier >= a->tier) ? VERTICAL_ACTUATOR_EXTENDING_FLAG : 0;

    // determine how many tiers are between the current and target tiers
    unsigned char tierDifference = extension ? nextTier - a->tier : a->tier - nextTier;

    // guard against bad nextTier values
    if (!tierDifference || nextTier >= a->maxTier)
        return;

    // ensure that actuator is in normal position before continuing
    if (a->isDropped)
        vertical_actuator_lift(a);

    // move to target tier in the normal position and update state
    vertical_actuator_actuate(a, extension, extension ? TIER_INTERVAL_UP_MS * tierDifference : TIER_INTERVAL_DOWN_MS * tierDifference);
    a->tier = nextTier;
}

// ensure that actuator is completely retracted by delaying longer
void vertical_actuator_home(VerticalActuatorA* a) {
    vertical_actuator_actuate(a, 0, TIER_INTERVAL_DOWN_MS << 1);
}