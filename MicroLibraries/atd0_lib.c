// *******************************************
//  Filename:       atd0_lib.c
//  Purpose:        Source file for atd0_lib.h
//  Processor:      MC9S12XDP512
//  Crystal:        16 MHz
//  Author:         Jonathan Melcher
//  Last Updated:   12/12/2015
// *******************************************

#include "atd0_lib.h"


// CTL2 default settings: 11100000 - ATD power up, fast flag (clears on read), ATD continues in wait wode
static const unsigned char CTL2_DEFAULT_SETTINGS = 0xE0;

// CTL3 default settings: 00000010 - 8 conversions per sequence, result into register, finish conversion before freeze
static const unsigned char CTL3_DEFAULT_SETTINGS = 0x02;

// CTL4 default settings: 00100110 - 1.75us period, 4 A/D conversion clock periods, 7us/sample, 10-bit resolution
static const unsigned char CTL4_DEFAULT_SETTINGS = 0x26;

// CTL5 default settings: 10110000 - right-justified, unsigned, continuous scan conversion, sample all 8 channels,
//                                   starts filling resultant register at the bottom, mapping Channelx to DRx
static const unsigned char CTL5_DEFAULT_SETTINGS = 0xB0;

static const unsigned char SEQUENCE_COMPLETE = 0x80;

void atd0_init(void) {

    ATD0DIEN = 0x00;                                // enable all inputs
    ATD0CTL2 = CTL2_DEFAULT_SETTINGS;               // apply default settings to CTL2

    asm LDX #134;                                   // require 50us delay before continuing
    asm DBNE X, *;

    ATD0CTL3 = CTL3_DEFAULT_SETTINGS;               // apply default settings to CTL3
    ATD0CTL4 = CTL4_DEFAULT_SETTINGS;               // apply default settings to CTL4
    ATD0CTL5 = CTL5_DEFAULT_SETTINGS;               // apply default settings to CTL5
}

unsigned int atd0_read(void) {

    unsigned int result;
    while (!(ATD0STAT0 & SEQUENCE_COMPLETE));       // wait for sequence complete flag
    
    result = ATD0DR0;
    return result;
}