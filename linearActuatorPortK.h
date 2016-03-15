// ****************************************************************
//  filename    :   linearActuatorPortK.h
//  purpose     :   header for linearActuatorPortK.c
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

#ifndef _LINEAR_ACTUATOR_PORTK_H_
#define _LINEAR_ACTUATOR_PORTK_H_

typedef struct LinearActuatorPortK {
    unsigned char state;
    unsigned char maxStates;
    unsigned long int stateIntervalMs;
    unsigned char isHighBits;
    unsigned char isOn;
    unsigned char isExtending;
} ActuatorK;

void actuator_init(ActuatorK* a, unsigned char isHighBits, unsigned char maxStates, unsigned long int stateIntervalMs);
void actuator_set_power(ActuatorK* a, unsigned char power);
void actuator_actuate(ActuatorK* a, unsigned char direction, unsigned long int ms);
void actuator_state_transition(ActuatorK* a, unsigned char state);
void actuator_sync(ActuatorK* a);

#endif