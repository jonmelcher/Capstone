// ****************************************************************
//  filename    :   linearActuatorVerticalPortK.h
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

#ifndef _TIMER_H_
#define _TIMER_H_
#include "timer.h"
#endif

#ifndef _LINEAR_ACTUATOR_VERTICAL_PORTK_H_
#define _LINEAR_ACTUATOR_VERTICAL_PORTK_H_

typedef struct LinearActuatorVerticalPortK {
    unsigned char tier;
    unsigned char maxTier;
    unsigned char isDropped;
    unsigned char isOn;
    unsigned char isExtending;
} VerticalActuatorK;

void vertical_actuator_init(VerticalActuatorK* a, unsigned char maxTier);
void vertical_actuator_sync(VerticalActuatorK* a);
void vertical_actuator_disable(VerticalActuatorK* a);
void vertical_actuator_drop(VerticalActuatorK* a);
void vertical_actuator_lift(VerticalActuatorK* a);
void vertical_actuator_actuate(VerticalActuatorK* a, unsigned char extensionFlag, unsigned long long int ms);
void vertical_actuator_transition_tier(VerticalActuatorK* a, unsigned char nextTier);

#endif