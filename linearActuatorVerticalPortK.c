// *****************************************************************************
//  filename    :   linearActuatorVerticalPortK.c
//  purpose     :   provide basic functionality for the vertical linear actuator
//                  using the GPIO pins along the upper nibble of PORTK
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016/03/15
// *****************************************************************************

#include "linearActuatorVerticalPortK.h"

static const unsigned char VERTICAL_ACTUATOR_MASK = 0x3F;
static const unsigned char VERTICAL_ACTUATOR_ON_FLAG = 0x80;
static const unsigned char VERTICAL_ACTUATOR_EXTENDING_FLAG = 0x40;
static const unsigned long long int DROP_INTERVAL_MS = 18750;           // corresponds to a 1.5 inch actuation
static const unsigned long long int TIER_INTERVAL_MS = 50000;           // corresponds to a 4 inch actuation

// function (VerticalActuatorK* a, unsigned char) -> void
// initializes struct
void vertical_actuator_init(VerticalActuatorK* a, unsigned char maxTier) {

    a->maxTier = maxTier;
    a->tier = 0;                // assume linear actuator is fully retracted
    a->isDropped = 1;           // meaning we are on bottom tier and in dropped position
    DDRK |= (VERTICAL_ACTUATOR_ON_FLAG | VERTICAL_ACTUATOR_EXTENDING_FLAG);
    vertical_actuator_disable(a);
}

// function (VerticalActuatorK*) -> void
// synchronizes PORTK with state of struct
void vertical_actuator_sync(VerticalActuatorK* a) {

    unsigned char current = PORTK;          // get copy of PORTK contents
    current &= VERTICAL_ACTUATOR_MASK;      // remove bits 7, 6
    current |= a->isOn | a->isExtending;    // send in state of isOn/isExtending to bits 7, 6
    PORTK = current;                        // update PORTK
}

// function (VerticalActuatorK*) -> void
// brings to an OFF position
void vertical_actuator_disable(VerticalActuatorK* a) {
    a->isOn = 0;
    a->isExtending = 0;
    vertical_actuator_sync(a);
}

// function (VerticalActuatorK*) -> void
// actuates into bottom of cell and brings to OFF position
void vertical_actuator_drop(VerticalActuatorK* a) {

    if (a->isDropped)
        return;
    vertical_actuator_actuate(a, 0, DROP_INTERVAL_MS);
    a->isDropped = 1;
}

// function (VerticalActuatorK*) -> void
// actuates into center of cell and brings to OFF position
void vertical_actuator_lift(VerticalActuatorK* a) {
    if (!a->isDropped)
        return;
    vertical_actuator_actuate(a, VERTICAL_ACTUATOR_EXTENDING_FLAG, DROP_INTERVAL_MS);
    a->isDropped = 0;
}

// function (VerticalActuatorK*, unsigned char, unsigned long long int) -> void
// actuates in the desired direction for the desired amount of milliseconds then brings to an OFF position
void vertical_actuator_actuate(VerticalActuatorK* a, unsigned char extensionFlag, unsigned long long int ms) {
    a->isOn = VERTICAL_ACTUATOR_ON_FLAG;
    a->isExtending = extensionFlag;
    vertical_actuator_sync(a);
    timer_delay_ms(ms);
    vertical_actuator_disable(a);
}

// function (VerticalActuatorK*, unsigned char) -> void
// actuates to next tier and brings to OFF position - will be in center of tier at end
void vertical_actuator_transition_tier(VerticalActuatorK* a, unsigned char nextTier) {

    unsigned char extension = (nextTier >= a->tier) ? VERTICAL_ACTUATOR_EXTENDING_FLAG : 0;
    unsigned char tierDifference = extension ? nextTier - a->tier : a->tier - nextTier;

    if (!tierDifference || nextTier > a->maxTier)
        return;
    if (a->isDropped)
        vertical_actuator_lift(a);

    vertical_actuator_actuate(a, extension, TIER_INTERVAL_MS * tierDifference);
    a->tier = nextTier;
}
