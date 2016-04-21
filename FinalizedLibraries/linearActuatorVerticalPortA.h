// ****************************************************************
//  filename    :   linearActuatorVerticalPortA.h
//  purpose     :   header for linearActuatorVerticalPortK.c
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016/03/15
// ****************************************************************

#ifndef _HIDEF_H_
#define _HIDEF_H_
#include <hidef.h>
#endif

#ifndef _DERIVATIVE_H_
#define _DERIVATIVE_H_
#include "derivative.h"
#endif

#ifndef _LINEAR_ACTUATOR_VERTICAL_PORTA_H_
#define _LINEAR_ACTUATOR_VERTICAL_PORTA_H_

typedef struct LinearActuatorVerticalPortA {
    unsigned char tier;
    unsigned char maxTier;
    unsigned char isDropped;
    unsigned char isOn;
    unsigned char isExtending;
} VerticalActuatorA;

void vertical_actuator_init(VerticalActuatorA* a);
void vertical_actuator_transition_tier(VerticalActuatorA* a, unsigned char nextTier);
void vertical_actuator_home(VerticalActuatorA* a);
void vertical_actuator_drop(VerticalActuatorA* a);
void vertical_actuator_lift(VerticalActuatorA* a);

#endif