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
#include "garageCommunication.h"
#include "garage.h"


static const unsigned char HOME = 0;
static const unsigned char CELLS_PER_TIER = 8;
static const unsigned long int DEGREES_BETWEEN_CELLS = 45;
static const unsigned char OUTGOING = 0x20;
static const unsigned char INCOMING = 0x21;
static const unsigned long long int DELAY_BETWEEN_MOVEMENTS_MS = 100;
static const unsigned char START_INSTRUCTION = 0xF0;
static const unsigned char CONTINUE_INSTRUCTION = 0x01;
static const unsigned char STOP_INSTRUCTION = 0xF1;
static const unsigned char INSTRUCTIONS_COMPLETED = 0x03;


// private prototypes
void move_to_cell(StepperA* motor, VerticalActuatorA* va, unsigned char cell);
void pickup_car_from_cell(VerticalActuatorA* va, HorizontalActuatorK* ha);
void dropoff_car_in_cell(VerticalActuatorA* va, HorizontalActuatorK* ha);


void get_instruction(Instruction* ins) {
    while (garage_rx() != START_INSTRUCTION);
    garage_tx(CONTINUE_INSTRUCTION);
    ins->cell = garage_rx();
    garage_tx(CONTINUE_INSTRUCTION);
    ins->direction = garage_rx();
    garage_tx(CONTINUE_INSTRUCTION);
    while (garage_rx() != STOP_INSTRUCTION);
}

void automation_process(StepperA* motor, VerticalActuatorA* va, HorizontalActuatorK* ha, Instruction* ins) {
    switch (ins->direction) {
        case OUTGOING:
            take_out_car(motor, va, ha, ins->cell);
            break;
        case INCOMING:
            take_in_car(motor, va, ha, ins->cell);
            break;
    }
    garage_tx(INSTRUCTIONS_COMPLETED);
}

void pickup_car_from_cell(VerticalActuatorA* va, HorizontalActuatorK* ha) {
    vertical_actuator_drop(va);                         // lower arm into 'dropped' position to reach under car
    timer_delay_ms(DELAY_BETWEEN_MOVEMENTS_MS);
    horizontal_actuator_extend(ha);                     // extend arm into cell to be underneath car
    timer_delay_ms(DELAY_BETWEEN_MOVEMENTS_MS);
    vertical_actuator_lift(va);                         // raise arm into 'normal' position, now supporting car
    timer_delay_ms(DELAY_BETWEEN_MOVEMENTS_MS);
    horizontal_actuator_retract(ha);                    // retract arm out of cell, now with car in tow
    timer_delay_ms(DELAY_BETWEEN_MOVEMENTS_MS);
}

void dropoff_car_in_cell(VerticalActuatorA* va, HorizontalActuatorK* ha) {
    vertical_actuator_lift(va);                         // ensure arm is in 'normal' position
    timer_delay_ms(DELAY_BETWEEN_MOVEMENTS_MS);
    horizontal_actuator_extend(ha);                     // extend into cell
    timer_delay_ms(DELAY_BETWEEN_MOVEMENTS_MS);
    vertical_actuator_drop(va);                         // send arm into 'dropped' position, no longer supporting car
    timer_delay_ms(DELAY_BETWEEN_MOVEMENTS_MS);
    horizontal_actuator_retract(ha);                    // retract out of cell
    timer_delay_ms(DELAY_BETWEEN_MOVEMENTS_MS);
    vertical_actuator_lift(va);                         // bring arm into 'normal' position
    timer_delay_ms(DELAY_BETWEEN_MOVEMENTS_MS);
}

void move_to_cell(StepperA* motor, VerticalActuatorA* va, unsigned char cell) {
    vertical_actuator_transition_tier(va, cell / CELLS_PER_TIER);
    timer_delay_ms(DELAY_BETWEEN_MOVEMENTS_MS);
    stepper_set_position(motor, stepper_degrees_to_position((cell % CELLS_PER_TIER) * DEGREES_BETWEEN_CELLS));
    timer_delay_ms(DELAY_BETWEEN_MOVEMENTS_MS);
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