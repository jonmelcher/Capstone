// ***************************************************************************
//  filename    :   garage.c
//  purpose     :   provide basic functionality for the combined movement in
//                  garage using the stepper motor and two linear actuators in
//                  order to deliver and retrieve cars from cells
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016/03/15
// ***************************************************************************


#include "timer.h"
#include "linearActuatorVerticalPortA.h"
#include "linearActuatorHorizontalPortK.h"
#include "stepperPortA.h"
#include "garage.h"


#define HOME 0


static const unsigned char CELLS_PER_TIER = 8;
static const unsigned char TIERS = 3;
static const unsigned long int DEGREES_BETWEEN_CELLS = 45;


void pickup_car_from_cell(VerticalActuatorA* va, HorizontalActuatorK* ha) {
    vertical_actuator_drop(va);
    horizontal_actuator_extend(ha);
    vertical_actuator_lift(va);
    horizontal_actuator_retract(ha);
}

void dropoff_car_in_cell(VerticalActuatorA* va, HorizontalActuatorK* ha) {
    horizontal_actuator_extend(ha);
    vertical_actuator_drop(va);
    horizontal_actuator_retract(ha);
    vertical_actuator_lift(va);
}

void move_to_cell(StepperA* motor, VerticalActuatorA* va, unsigned char cell) {
    unsigned char tier = cell / CELLS_PER_TIER;
    unsigned long int position = (cell % CELL_PER_TIER) * DEGREES_BETWEEN_CELLS;
    vertical_actuator_transition_tier(va, tier);        // move if necessary to appropriate tier
    stepper_set_position(motor, position);              // rotate if necessary to appropriate cell
}

void take_in_car(StepperA* motor, VerticalActuatorA* va, HorizontalActuatorK* ha, unsigned char cell) {
    pickup_car_from_cell(va, ha);                       // pick up car from RFID cell
    move_to_cell(motor, va, cell);                      // move to appropriate cell
    dropoff_car_in_cell(va, ha);                        // drop off car into chosen cell
    move_to_cell(motor, va, HOME);                      // go to RFID cell
}

void take_out_car(StepperA* motor, VerticalActuatorA* va, HorizontalActuatorK* ha, unsigned char cell) {
    move_to_cell(motor, va, cell);                      // move to appropriate cell
    pickup_car_from_cell(va, ha);                       // pick up car
    move_to_cell(motor, va, HOME);                      // move to RFID cell
    dropoff_car_in_cell(va, ha);                        // drop off car
}