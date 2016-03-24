// ********************************************************************************************
//  filename    :   stepperATest.c
//  purpose     :   provide a comprehensive testing routine for the functionality of stepperA.c
//                  which employs a 5V 6-lead unipolar stepper motor
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016-03-24
// ********************************************************************************************


#include "timer.h"
#include "stepperPortA.h"


static const unsigned long long int DELAY_BETWEEN_TESTS_MS = 5000;
static const unsigned long int DEGREES_BETWEEN_CELLS = 45;
static const unsigned long int DEGREES_IN_A_CIRCLE = 360;
static const unsigned char SAMPLE_SIZE = 5;


void circle_check(StepperA* motor);
void rotation_check(StepperA* motor);


void main(void) {

    StepperA motor;

    timer_init_8us();
    stepper_init(&motor);
    _DISABLE_COP();

    for (;;) {
        circle_check(&motor);
        rotation_check(&motor);
    }
}

void circle_check(StepperA* motor) {

    unsigned char i;

    stepper_set_direction(motor, 1);        // sets direction to clockwise
    for (i = 0; i < SAMPLE_SIZE; ++i) {
        stepper_circle(motor);
        timer_delay_ms(DELAY_BETWEEN_TESTS_MS);
    }

    stepper_set_direction(motor, 0);        // sets direction to counter-clockwise
    for (i = 0; i < SAMPLE_SIZE; ++i) {
        stepper_circle(motor);
        timer_delay_ms(DELAY_BETWEEN_TESTS_MS);
    }
}

void rotation_check(StepperA* motor) {

    unsigned long int degrees;

    stepper_set_direction(motor, 1);        // sets direction to clockwise

    // go back and forth between the initial position and each cell location
    for (degrees = DEGREES_BETWEEN_CELLS; degrees < DEGREES_IN_A_CIRCLE; degrees += DEGREES_BETWEEN_CELLS) {
        stepper_rotate(motor, degrees);
        timer_delay_ms(DELAY_BETWEEN_TESTS_MS >> 1);
        stepper_set_position(motor, 0);
        timer_delay_ms(DELAY_BETWEEN_TESTS_MS >> 1);
    }

    stepper_set_direction(motor, 0);        // sets direction to counter-clockwise

    // go back and forth between the initial position and each cell location
    for (degrees = DEGREES_BETWEEN_CELLS; degrees < DEGREES_IN_A_CIRCLE; degrees += DEGREES_BETWEEN_CELLS) {
        stepper_rotate(motor, degrees);
        timer_delay_ms(DELAY_BETWEEN_TESTS_MS >> 1);
        stepper_set_position(motor, 0);
        timer_delay_ms(DELAY_BETWEEN_TESTS_MS >> 1);
    }
}
