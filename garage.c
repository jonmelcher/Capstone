// **************************************************************************************************************
//  filename    :   garage.c
//  purpose     :   interface stepperA motor, horizontalActuatorK actuator and verticalActuatorA actuator
//                  together to allow client to move cars between the entrance cell and other cells in the garage
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016/03/17
// **************************************************************************************************************


#include "timer.h"
#include "linearActuatorVerticalPortA.h"
#include "linearActuatorHorizontalPortK.h"
#include "stepperPortA.h"
#include "garage.h"


static const unsigned char HOME = 0;
static const unsigned char CELLS_PER_TIER = 8;
static const unsigned long int DEGREES_BETWEEN_CELLS = 45;


void pickup_car_from_cell(VerticalActuatorA* va, HorizontalActuatorK* ha) {
    vertical_actuator_drop(va);                         // send arm into 'dropped' position to reach under car
    horizontal_actuator_extend(ha);                     // extend into cell
    vertical_actuator_lift(va);                         // send arm into 'normal' position, now supporting car
    horizontal_actuator_retract(ha);                    // retract out of cell
}

void dropoff_car_in_cell(VerticalActuatorA* va, HorizontalActuatorK* ha) {
    vertical_actuator_lift(va);                         // ensure arm is in 'normal' position
    horizontal_actuator_extend(ha);                     // extend into cell
    vertical_actuator_drop(va);                         // send arm into 'dropped' position, no longer supporting car
    horizontal_actuator_retract(ha);                    // retract out of cell
    vertical_actuator_lift(va);                         // bring arm into 'normal' position
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
