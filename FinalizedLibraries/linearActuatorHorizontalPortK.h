// ****************************************************************
//  filename    :   linearActuatorHorizontalPortK.h
//  purpose     :   header for linearActuatorHorizontalPortK.c
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

#ifndef _LINEAR_ACTUATOR_HORIZONTAL_PORTK_H_
#define _LINEAR_ACTUATOR_HORIZONTAL_PORTK_H_

typedef struct LinearActuatorHorizontalPortK {
    unsigned char isOn;
    unsigned char isExtended;
    unsigned char isExtending;
} HorizontalActuatorK;

void horizontal_actuator_init(HorizontalActuatorK* a);
void horizontal_actuator_retract(HorizontalActuatorK* a);
void horizontal_actuator_extend(HorizontalActuatorK* a);
void horizontal_actuator_home(HorizontalActuatorK* a);

#endif