// ****************************************************************
//  filename    :   stepperTest.c
//  purpose     :   comprehensive test procedure for stepperPortA.c
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016/03/15
// ****************************************************************

#include "stepperPortA.h"

static const unsigned char SAMPLE_SIZE = 5;
static const unsigned long long int TEST_DELAY = 25000;

void test(void (*fn)(StepperA*), StepperA* motor);
void test2(void (*fn)(StepperA*, unsigned long int), StepperA* motor, unsigned long int degrees);
void toggle_circle(StepperA* motor);
void rotate_home(StepperA* motor);
void circle_test(StepperA* motor);

void main(void) {

    StepperA motor;
    stepper_init(&motor);

    // runs forever
    for (;;) {
        circle_test(motor);
        stepper_set_direction(motor, 1);
        rotate_test(motor);
        stepper_set_direction(motor, 0);
        rotate_test(motor);
    }
}

void test(void (*fn)(StepperA*), StepperA* motor) {

    unsigned char sample;
    unsigned long long int ticks;

    for (sample = 0; sample < SAMPLE_SIZE; ++sample)
        fn(motor);
    for (ticks = 0; ticks < TEST_DELAY; ++ticks);
}

void test2(void (*fn)(StepperA*, unsigned long int), StepperA* motor, unsigned long int degrees) {

    unsigned char sample;
    unsigned long long int ticks;

    for (sample = 0; sample < SAMPLE_SIZE; ++sample)
        fn(motor, degrees);
    for (ticks = 0; ticks < TEST_DELAY; ++ticks);
}

void toggle_circle(StepperA* motor) {
    stepper_toggle_direction(motor);
    stepper_circle(motor);
}

void circle_test(StepperA* motor) {

    // set direction to clockwise
    stepper_set_direction(motor, 1);
    test(stepper_circle, motor);

    // set direction to counter-clockwise
    stepper_set_direction(motor, 0);
    test(stepper_circle, motor);

    // alternate directions
    test(toggle_circle, motor);
}

void rotate_home(StepperA* motor, unsigned long int degrees) {

    stepper_rotate(motor, degrees);
    stepper_home(motor);
}

void rotate_test(StepperA* motor) {

    unsigned long int degrees = 0;

    while (degrees < 720) {
        test2(rotate_home, motor, degrees);
        degrees += 45;
    }
}