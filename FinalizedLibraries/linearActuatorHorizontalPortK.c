// *******************************************************************************
//  filename    :   linearActuatorHorizontalPortK.c
//  purpose     :   provide basic functionality for the horizontal linear actuator
//                  using the GPIO pins along the upper nibble of PORTK
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016/03/15
// *******************************************************************************


#include "timer.h"
#include "linearActuatorHorizontalPortK.h"


// private prototypes
void horizontal_actuator_sync(HorizontalActuatorK* a);
void horizontal_actuator_disable(HorizontalActuatorK* a);
void horizontal_actuator_actuate(HorizontalActuatorK* a, unsigned char directionFlag, unsigned long long int ms);


// PIN 19 - POWER (ON / OFF)
// PIN 20 - DIRECTION (EXTENDING / RETRACTING)
// inverse of horizontal actuator bits
static const unsigned char HORIZONTAL_ACTUATOR_MASK = 0xCF;

// signal to send to actuator circuit to enable current
static const unsigned char HORIZONTAL_ACTUATOR_ON_FLAG = 0x20;

// signal to send to actuator circuit to enable retraction
static const unsigned char HORIZONTAL_ACTUATOR_EXTENDING_FLAG = 0x10;

// amount of time in milliseconds to guarantee 4 inch extension/retraction
// (22.5 seconds * 0.2 inches / second = 4.5 inches)
static const unsigned long long int ACTUATION_INTERVAL_MS = 26000;

// amount of time in milliseconds to delay after disabling actuator
static const unsigned long long int STOP_DELAY_MS = 1000;

// initializes PORTK and state of actuator to be off and retracting
// ensure that the real-life counterpart is fully retracted at this point!
void horizontal_actuator_init(HorizontalActuatorK* a) {
    a->isOn = 0;
    a->isExtended = 0;
    a->isExtending = 0;
    DDRK |= ~(HORIZONTAL_ACTUATOR_MASK);
    horizontal_actuator_sync(a);
}

// fully retracts actuator if extended
void horizontal_actuator_retract(HorizontalActuatorK* a) {
    if (!a->isExtended)
        return;
    horizontal_actuator_actuate(a, 0, ACTUATION_INTERVAL_MS);
    a->isExtended = 0;
}

// fully extends actuator if retracted
void horizontal_actuator_extend(HorizontalActuatorK* a) {
    if (a->isExtended)
        return;
    horizontal_actuator_actuate(a, HORIZONTAL_ACTUATOR_EXTENDING_FLAG, ACTUATION_INTERVAL_MS);
    a->isExtended = 1;
}

// fully retracts actuator and guarantees to be at stop limit
void horizontal_actuator_home(HorizontalActuatorK* a) {
    horizontal_actuator_actuate(a, 0, (ACTUATION_INTERVAL_MS * 3) >> 1);
    a->isExtended = 0;
}

// (private) synchronizes PORTK with current state of struct
void horizontal_actuator_sync(HorizontalActuatorK* a) {
    unsigned char currentState = PORTK;             // get copy of PORTK contents
    currentState &= HORIZONTAL_ACTUATOR_MASK;       // remove bits 5, 4
    currentState |= a->isOn | a->isExtending;       // send in state of isOn/isExtending to bits 5, 4
    PORTK = currentState;                           // update PORTK
}

// (private) turns off actuator
void horizontal_actuator_disable(HorizontalActuatorK* a) {
    a->isOn = 0;
    horizontal_actuator_sync(a);
    timer_delay_ms(STOP_DELAY_MS);                  // delay after stopping before continuing
}

// (private) actuates actuator in desired direction for the desired time
void horizontal_actuator_actuate(HorizontalActuatorK* a, unsigned char directionFlag, unsigned long long int ms)
{
    // set up future actuation state for actuator and synchronize
    a->isOn = HORIZONTAL_ACTUATOR_ON_FLAG;
    a->isExtending = directionFlag;
    horizontal_actuator_sync(a);
    timer_delay_ms(ms);
    horizontal_actuator_disable(a); 
}