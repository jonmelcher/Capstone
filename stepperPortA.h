// ******************************************
//  filename    :   stepperPortA.h
//  purpose     :   header for stepperPortA.c
//
//  written by Jonathan Melcher on 2016/03/03
// ******************************************

#ifndef _HIDEF_H_
#define _HIDEF_H_
#include <hidef.h>
#endif

#ifndef _DERIVATIVE_H_
#define _DERIVATIVE_H_
#include "derivative.h"
#endif

#ifndef _STEPPER_PORTA_H_
#define _STEPPER_PORTA_H_

const unsigned char STATES = 4;

typedef struct StepperPortA {
    unsigned char states[STATES];
    unsigned char state;
    unsigned char isClockwise;
    unsigned long int steps;
} StepperA;

void stepper_init(StepperA* motor);
void stepper_sync(StepperA* motor);
void stepper_toggle_direction(StepperA* motor);
void stepper_step(StepperA* motor);
void stepper_circle(StepperA* motor);
void stepper_rotate(StepperA* motor, unsigned long int degrees);

#endif