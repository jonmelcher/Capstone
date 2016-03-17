// ***************************************************************************
//  filename    :   linearActuatorVerticalPortK.c
//  purpose     :   provide basic functionality for a timer using the built-in
//                  Enhanced Capture Timer
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016/03/16
// ***************************************************************************

#include "timer.h"

void timer_init_8us(void) {
    TSCR1 |= 0x80;      // enable timer module
    TSCR2 &= 0xF8;      // set prescale to Bus/64 (8 us)
    TSCR2 |= 0x06;      // ... continued
    TIOS |= 0x01;       // set IOS0 to output compare
    TCTL2 &= 0xFC;      // set PT0 to toggle mode
    TCTL2 |= 0x01;      // ... continued
    TFLG |= 0x01;       // clear flag
}

void timer_delay_ms(unsigned long long int ms) {

    unsigned long long int count;
    TC0 = TCNT + 125;
    for (count = 0; count < ms; ++count) {
        TFLG1 |= 0x01;
        while (!(TFLG1 & 0x01));
        TC0 += 125;
    }
}