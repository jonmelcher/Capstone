#include "timer.h"
#include "linearActuatorVerticalPortA.h"
#include "linearActuatorHorizontalPortK.h"
#include "stepperPortA.h"
#include "garageCommunication.h"
#include "garage.h"

static const unsigned char TIERS = 3;
static const unsigned char START_INSTRUCTION = 0xF0;
static const unsigned char HOME = 0;
static const unsigned char CELLS_PER_TIER = 8;
static const unsigned long int DEGREES_BETWEEN_CELLS = 45;
static const unsigned char OUTGOING = 0x20;
static const unsigned char INCOMING = 0x21;
static const unsigned long long int DELAY_BETWEEN_MOVEMENTS_MS = 500;
static const unsigned char CONTINUE_INSTRUCTION = 0x01;
static const unsigned char STOP_INSTRUCTION = 0x02;
static const unsigned char INSTRUCTIONS_COMPLETED = 0x03;


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
    vertical_actuator_init(&vertical, TIERS);       // initalize vertical linear actuator interface

    for(;;) {
        if (!is_garage_rx_ready() || garage_rx() != START_INSTRUCTION)
            continue;

        get_instruction(&instruction);
        automation_process(&rotational, &vertical, &horizontal, &instruction);
    }
}