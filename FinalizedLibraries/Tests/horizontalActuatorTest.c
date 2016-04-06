// ************************************************************************************************
//  filename    :   horizontalActuatorTest.c
//  purpose     :   provide a comprehensive testing routine for the functionality of
//                  linearActuatorHorizontalPortK.c, which employs a 12V bipolar 4" linear actuator
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016-03-24
// ************************************************************************************************


#include "timer.h"
#include "linearActuatorHorizontalPortK.h"


static const unsigned long long int DELAY_BETWEEN_TESTS_MS = 5000;
static const unsigned char SAMPLE_SIZE = 2;


void extension_retraction_check(HorizontalActuatorK* actuator);


void main(void) {

    HorizontalActuatorK actuator;

    timer_init_8us();
    horizontal_actuator_init(&actuator);
    _DISABLE_COP();

    for (;;)
        extension_retraction_check(&actuator);
}

void extension_retraction_check(HorizontalActuatorK* actuator) {

    unsigned char i;

    for (i = 0; i < SAMPLE_SIZE; ++i) {
        horizontal_actuator_extend(actuator);
        timer_delay_ms(DELAY_BETWEEN_TESTS_MS);
        horizontal_actuator_retract(actuator);
        timer_delay_ms(DELAY_BETWEEN_TESTS_MS);
    }
}