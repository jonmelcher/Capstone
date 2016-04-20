// **********************************************************************
//  filename    :   stepperPortA.c
//  purpose     :   provide basic functionality for a stepper motor using
//                  the GPIO pins along the upper nibble of PORTA
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016/03/03
// **********************************************************************


#include "stepperPortA.h"


// private prototypes
void stepper_delay(StepperA* motor);
void stepper_release_torque(StepperA* motor);
unsigned char stepper_get_optimal_direction(StepperA* motor, unsigned long int position);
unsigned long int stepper_get_steps(StepperA* motor, unsigned char direction, unsigned long int position);
void stepper_steps(StepperA* motor, unsigned long int steps);
void stepper_sync(StepperA* motor);


// directional constants
static const unsigned char CLOCKWISE = 1;
static const unsigned char COUNTERCLOCKWISE = 0;

// amount of steps the motor must take to move the rotational platform 360 degrees
static const unsigned long int MAX_STEPS = 400;

// number of transition states of the stepper motor (half steps)
static const unsigned char STEPPER_STATES = 8;

static const unsigned long int STEPPER_DELAY_MIN = 2000;
static const unsigned long int STEPPER_DELAY_MAX = 3500;

// number of degrees per state transition (0.9) * 10
static const unsigned char STEPPER_RATIO = 9;

// factor used in the STEPPER_RATIO
static const unsigned char RATIO_FACTOR = 10;

// degrees in circle
static const unsigned long int DEGREES_IN_CIRCLE = 360;

// stepper output bits
static const unsigned char STEPPER_BITS = 0xF0;

// initializes motor values, and array depicting cycle pattern for stepper
// motor defaults to clockwise movement
void stepper_init(StepperA* motor) {
    motor->states[0] = 0xEF;                            // 1110....     RED WIRE
    motor->states[1] = 0xCF;                            // 1100....     HALF-STEP
    motor->states[2] = 0xDF;                            // 1101....     BROWN WIRE
    motor->states[3] = 0x9F;                            // 1001....     HALF-STEP
    motor->states[4] = 0xBF;                            // 1011....     GREY WIRE
    motor->states[5] = 0x3F;                            // 0011....     HALF-STEP
    motor->states[6] = 0x7F;                            // 0111....     ORANGE WIRE
    motor->states[7] = 0x6F;                            // 0110....     HALF-STEP
    motor->state = 0;                                   // current state of motor (reflects grounded bit in upper nibble)
    motor->steps = 0;                                   // assume motor is at start position
    motor->direction = CLOCKWISE;                       // assume motor starts in clockwise direction
    motor->delay = STEPPER_DELAY_MAX;                   // sets delay to maximum
    DDRA |= STEPPER_BITS;                               // turns on outputs for GPIO
    PORTA |= STEPPER_BITS;                              // syncs PORTA with stepper
}

// toggles direction stepper cycles in
void stepper_toggle_direction(StepperA* motor) {
    motor->direction = (motor->direction == CLOCKWISE) ? COUNTERCLOCKWISE : CLOCKWISE;
}

// sets direction stepper cycles in
void stepper_set_direction(StepperA* motor, unsigned char direction) {
    motor->direction = direction ? CLOCKWISE : COUNTERCLOCKWISE;
}

// steps motor in whichever direction, updates state and syncs motor
void stepper_step(StepperA* motor) {
    if (motor->direction == CLOCKWISE) {
        motor->steps = (motor->steps + 1) % MAX_STEPS;
        motor->state = (motor->state + 1) % STEPPER_STATES;
    }
    else {
        motor->steps = (motor->steps) ? motor->steps - 1 : MAX_STEPS - 1;
        motor->state = (motor->state) ? motor->state - 1 : STEPPER_STATES - 1;
    }
    stepper_sync(motor);
}

// causes stepper to rotate the rotational platform 360 degrees
void stepper_circle(StepperA* motor) {
    stepper_steps(motor, MAX_STEPS);
}

// causes stepper to rotate rotational platform a specified number of degrees
// note that degrees are always positive; negative we can simply toggle direction and rotate
void stepper_rotate(StepperA* motor, unsigned long int degrees) {
    stepper_steps(motor, ((degrees % DEGREES_IN_CIRCLE) * RATIO_FACTOR) / STEPPER_RATIO);
}

// causes stepper to rotate back to starting position in whichever direction is quickest
void stepper_home(StepperA* motor) {
    stepper_set_position(motor, 0);
}

// calculates optimal path to rotate and rotates to given position
void stepper_set_position(StepperA* motor, unsigned long int position) {
    unsigned char oldDirection = motor->direction;
    motor->direction = stepper_get_optimal_direction(motor, position);
    stepper_steps(motor, stepper_get_steps(motor, motor->direction, position));
    motor->direction = oldDirection;
}

// (private) sets upper nibble of PORTA to reflect current motor-state for a time interval before releasing torque
void stepper_sync(StepperA* motor) {
    unsigned char currentState = PORTA;                 // gets copy of contents of PORTA
    currentState |= STEPPER_BITS;                       // sets all of upper nibble
    currentState &= motor->states[motor->state];        // masks upper nibble with current motor state
    PORTA = currentState;                               // PORTA is synced with motor
    stepper_delay(motor);                               // delay before torque released
    stepper_release_torque(motor);
}

// (private) steps given number of steps with speed up / slow down
void stepper_steps(StepperA* motor, unsigned long int steps) {
    unsigned long int step_difference;
    unsigned long int i;
    switch (steps) {
        case 0:
            break;
        case 1:
            stepper_sync(motor);
            stepper_step(motor);
            break;
        default:
            step_difference = (STEPPER_DELAY_MAX - STEPPER_DELAY_MIN) / steps;
            stepper_sync(motor);
            for (i = 0; i < steps >> 1; ++i) {
                stepper_step(motor);
                motor->delay -= step_difference;
            }
            for (i = 0; i < steps >> 1; ++i) {
                stepper_step(motor);
                motor->delay += step_difference;
            }
            break;
    }

    for (i = 0; i < STEPPER_DELAY_MAX >> 1; ++i) ;
    stepper_release_torque(motor);
}

// (private) gets number of steps it would take to get to the given position in the given direction
unsigned long int stepper_get_steps(StepperA* motor, unsigned char direction, unsigned long int position) {
    position %= MAX_STEPS;
    if (motor->steps >= position)
        return (direction == CLOCKWISE) ? MAX_STEPS - motor->steps + position : motor->steps - position;
    else
        return (direction == CLOCKWISE) ? position - motor->steps : MAX_STEPS - position + motor->steps;
}

// (private) chooses the optimal direction to step in to reach position from current position
unsigned char stepper_get_optimal_direction(StepperA* motor, unsigned long int position) {
    unsigned long int clockwise = stepper_get_steps(motor, CLOCKWISE, position);
    unsigned long int counterClockwise = stepper_get_steps(motor, COUNTERCLOCKWISE, position);
    return (clockwise < counterClockwise) ? CLOCKWISE : COUNTERCLOCKWISE;
}

// (private) releases ground on all motor leads
void stepper_release_torque(StepperA* motor) {
    PORTA |= STEPPER_BITS;
}

// (private) manual delay based on struct delay field
void stepper_delay(StepperA* motor) {
    unsigned long int i;
    for (i = 0; i < motor->delay; ++i);
}
