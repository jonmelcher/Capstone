// ****************************************************************
//  filename    :   stepperPortA.h
//  purpose     :   header for stepperPortA.c
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016/03/03
// ****************************************************************

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

typedef struct StepperPortA {
    unsigned char states[8];
    unsigned char state;
    unsigned char direction;
    unsigned long int steps;
    unsigned long int delay;
} StepperA;

void stepper_init(StepperA* motor);
void stepper_toggle_direction(StepperA* motor);
void stepper_set_direction(StepperA* motor, unsigned char direction);
void stepper_step(StepperA* motor);
void stepper_home(StepperA* motor);
void stepper_circle(StepperA* motor);
void stepper_rotate(StepperA* motor, unsigned long int degrees);
void stepper_set_position(StepperA* motor, unsigned long int position);
unsigned long int stepper_degrees_to_position(unsigned long int degrees);

#endif