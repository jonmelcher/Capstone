// *******************************************************************************
//  filename    :   linearActuatorHorizontalPortK.c
//  purpose     :   provide basic functionality for the horizontal linear actuator
//                  using the GPIO pins along the upper nibble of PORTK
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016/03/15
// *******************************************************************************

#include "linearActuatorHorizontalPortK.h"

static const unsigned char HORIZONTAL_ACTUATOR_MASK = 0xCF;
static const unsigned char HORIZONTAL_ACTUATOR_ON_FLAG = 0x20;
static const unsigned char HORIZONTAL_ACTUATOR_EXTENDING_FLAG = 0x10;
static const unsigned long long int EXTENSION_INTERVAL_MS = 50000;          // corresponds to a 4 inch actuation

void horizontal_actuator_init(HorizontalActuatorK* a) {

    a->isExtended = 0;
    horizontal_actuator_disable(a);
}

void horizontal_actuator_sync(HorizontalActuatorK* a) {

    unsigned char current = PORTK;          // get copy of PORTK contents
    current &= HORIZONTAL_ACTUATOR_MASK;    // remove bits 5, 4
    current |= a->isOn | a->isExtending;    // send in state of isOn/isExtending to bits 5, 4
    PORTK = current;                        // update PORTK
}

void horizontal_actuator_disable(HorizontalActuatorK* a) {
    a->isOn = 0;
    a->isExtending = 0;
    horizontal_actuator_sync(a);
}

void horizontal_actuator_extend(HorizontalActuatorK* a) {

    if (a->isExtended)
        return;

    a->isOn = HORIZONTAL_ACTUATOR_ON_FLAG;
    a->isExtending = HORIZONTAL_ACTUATOR_EXTENDING_FLAG;
    horizontal_actuator_sync(a);
    timer_delay_ms(EXTENSION_INTERVAL_MS);
    horizontal_actuator_disable(a);
    a->isExtended = 1;
}

void horizontal_actuator_retract(HorizontalActuatorK* a) {

    if (!a->isExtended)
        return;

    a->isOn = HORIZONTAL_ACTUATOR_ON_FLAG;
    a->isExtending = 0;
    horizontal_actuator_sync(a);
    timer_delay_ms(EXTENSION_INTERVAL_MS);
    horizontal_actuator_disable(a);
    a->isExtended = 0;
}