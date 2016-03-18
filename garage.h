// ****************************************************************
//  filename    :   garage.h
//  purpose     :   header file for garage.c
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016/03/17
// ****************************************************************

#ifndef _GARAGE_H_
#define _GARAGE_H_

void pickup_car_from_cell(VerticalActuatorA* va, HorizontalActuatorK* ha);
void dropoff_car_in_cell(VerticalActuatorA* va, HorizontalActuatorK* ha);
void move_to_cell(StepperA* motor, VerticalActuatorA* va, unsigned char cell);
void take_in_car(StepperA* motor, VerticalActuatorA* va, HorizontalActuatorK* ha, unsigned char cell);
void take_out_car(StepperA* motor, VerticalActuatorA* va, HorizontalActuatorK* ha, unsigned char cell);

#endif