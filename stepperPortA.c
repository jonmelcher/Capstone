// **********************************************************************
//  filename    :   stepperPortA.c
//  purpose     :   provide basic functionality for a stepper motor using
//                  the GPIO pins along the upper nibble of PORTA
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016/03/03
// **********************************************************************


#include "stepperPortA.h"


#define CLOCKWISE 1
#define COUNTERCLOCKWISE 0


// amount of steps the motor must take to move the rotational platform 360 degrees
static const unsigned long int MAX_STEPS = 400;

// number of transition states of the stepper motor (half steps)
static const unsigned char STEPPER_STATES = 8;

// amount of delay (ticks) between changes to PORTA
static const unsigned long int STEPPER_DELAY = 1500;

// number of degrees per state transition (0.9) * 10
static const unsigned char STEPPER_RATIO = 9;

// factor used in the STEPPER_RATIO
static const unsigned char RATIO_FACTOR = 10;

// degrees in circle
static const unsigned long int DEGREES_IN_CIRCLE = 360;

// stepper output bits
static const unsigned char STEPPER_BITS = 0xF0;

// function (StepperA*) -> void
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

    DDRA |= STEPPER_BITS;                               // turns on outputs for GPIO
    PORTA |= STEPPER_BITS;                              // syncs PORTA with stepper
}

// function (StepperA*) -> void
// clears and sets upper nibble of PORTA to reflect current motor state
void stepper_sync(StepperA* motor) {

    unsigned long int i;
    unsigned char currentState = PORTA;                 // gets copy of contents of PORTA
    currentState |= STEPPER_BITS;                       // sets all of upper nibble
    currentState &= motor->states[motor->state];        // syncs upper nibble of motor state to current

    PORTA = current;                                    // PORTA is synced with motor
    for (i = 0; i < STEPPER_DELAY; ++i);
    PORTA |= STEPPER_BITS;                              // torque is released
    for (i = 0; i < STEPPER_DELAY; ++i); 
}

// function (StepperA*) -> void
// toggles direction stepper cycles in
void stepper_toggle_direction(StepperA* motor) {
    motor->direction = (motor->direction == CLOCKWISE) ? COUNTERCLOCKWISE : CLOCKWISE;
}

// function (StepperA*) -> void
// sets direction stepper cycles in
void stepper_set_direction(StepperA* motor, unsigned char direction) {
    motor->direction = direction;
}

// function (StepperA*) -> void
// steps motor in whichever direction, updates state and syncs motor
void stepper_step(StepperA* motor) {

    if (motor->direction == CLOCKWISE) {
        motor->steps = (motor->steps + 1) % MAX_STEPS;
        motor->state = (motor->state + 1) % STEPPER_STATES;
    }
    else {
        motor->steps = (motor->steps + MAX_STEPS - 1) % MAX_STEPS;
        motor->state = (motor->state + STEPPER_STATES - 1) % STEPPER_STATES;
    }

    stepper_sync(motor);
}

// function (StepperA*) -> void
// causes stepper to rotate the rotational platform 360 degrees
void stepper_circle(StepperA* motor) {

  unsigned long int count = 0;
  
  do
      stepper_step(motor);
  while (++count < MAX_STEPS);
}

// function (StepperA*, unsigned long int) -> void
// causes stepper to rotate rotational platform a specified number of degrees
// note that degrees are always positive; negative we can simply toggle direction and rotate
void stepper_rotate(StepperA* motor, unsigned long int degrees) {
    
    unsigned long long int d = degrees;
    unsigned long long int steps;
    unsigned long long int i;
    d %= DEGREES_IN_CIRCLE;
    
    steps = (d * RATIO_FACTOR) / STEPPER_RATIO;
    for (i = 0; i < steps; ++i)
        stepper_step(motor);
}

// function (StepperA*) -> void
// causes stepper to rotate back to starting position in whichever direction is quickest
// note that the earlier direction state is not retained (can easily change this if nec.)
void stepper_home(StepperA* motor) {

    stepper_set_direction(motor, stepper_get_optimal_direction(motor, 0));
    while (motor->steps)
        stepper_step(motor);
    stepper_set_direction(CLOCKWISE);
}

// function (StepperA*, unsigned long int) -> void
// chooses the optimal direction to step in to reach position from current position
void stepper_get_optimal_direction(StepperA* motor, unsigned long int position) {

    unsigned long int clockwise;
    unsigned long int counterClockwise;

    if (motor->steps >= position) {
        counterClockwise = motor->steps - position;
        clockwise = MAX_STEPS - motor->steps + position;
    }
    else {
        clockwise = position - motor->steps;
        counterClockwise = MAX_STEPS - position + motor->steps;
    }

    return (clockwise >= counterClockwise) ? CLOCKWISE : COUNTERCLOCKWISE;
}