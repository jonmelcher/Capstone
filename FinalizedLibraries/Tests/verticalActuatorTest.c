// ***********************************************************************************************
//  filename    :   verticalActuatorTest.c
//  purpose     :   provide a comprehensive testing routine for the functionality of
//                  linearActuatorVerticalPortA.c, which employs a 12V bipolar 12" linear actuator
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016-03-24
// ***********************************************************************************************


#include "timer.h"
#include "linearActuatorVerticalPortA.h"


static const unsigned long long int DELAY_BETWEEN_TESTS_MS = 5000;
static const unsigned char SAMPLE_SIZE = 2;


void lift_drop_check(VerticalActuatorA* actuator);
void tier_check(VerticalActuatorA* actuator);


void main(void) {

    VerticalActuatorA actuator;

    timer_init_8us();
    vertical_actuator_init(&actuator, 3);
    _DISABLE_COP();

    for (;;) {
        lift_drop_check(&actuator);
        tier_check(&actuator);
    }
}

void lift_drop_check(VerticalActuatorA* actuator) {

    unsigned char i;

    for (i = 0; i < SAMPLE_SIZE; ++i) {
        vertical_actuator_lift(actuator);
        timer_delay_ms(DELAY_BETWEEN_TESTS_MS);
        vertical_actuator_drop(actuator);
        timer_delay_ms(DELAY_BETWEEN_TESTS_MS);
    }
}

void tier_check(VerticalActuatorA* actuator) {

    unsigned char tier;

    for (tier = 1; tier < actuator->maxTier; ++tier) {
        vertical_actuator_transition_tier(actuator, tier);
        timer_delay_ms(DELAY_BETWEEN_TESTS_MS);
        vertical_actuator_drop(actuator);
        timer_delay_ms(DELAY_BETWEEN_TESTS_MS);
        vertical_actuator_transition_tier(actuator, 0);
        timer_delay_ms(DELAY_BETWEEN_TESTS_MS);
    }
}
