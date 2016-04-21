#include "timer.h"
#include "linearActuatorVerticalPortA.h"
#include "linearActuatorHorizontalPortK.h"
#include "stepperPortA.h"
#include "garageCommunication.h"
#include "garage.h"


void main(void) {

    StepperA rotational;                            // 5V 6-lead unipolar stepper motor interface
    HorizontalActuatorK horizontal;                 // 12V bipolar 4" linear actuator interface
    VerticalActuatorA vertical;                     // 12V bipolar 12" linear actuator interface
    Instruction instruction;                        // structure for maintaining instructions from C# app

    _DISABLE_COP();
    timer_init_8us();                               // initialize enhanced capture timer
    garage_communication_init();                    // initialize serial communication between micro and C# app
    stepper_init(&rotational);                      // initialize rotational stepper motor interface
    horizontal_actuator_init(&horizontal);          // initialize horizontal linear actuator interface
    vertical_actuator_init(&vertical);              // initalize vertical linear actuator interface

    for(;;) {

        // retrieve instruction from C# app
        get_instruction(&instruction);

        // process instruction
        automation_process(&rotational, &vertical, &horizontal, &instruction);
    }
}