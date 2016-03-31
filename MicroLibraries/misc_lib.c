// ******************************************************
//  Filename:       misc_lib.c
//  Purpose:        Source file for misc library
//  Processor:      MC9S12XDP512
//  Crystal:        16 MHz
//  Author:         Jonathan Melcher
//  Last Updated:   20/11/2015
// ******************************************************


#include "misc_lib.h"

static const unsigned int MAX_HEX_TO_BCD_ENCODE = 10000;
static const unsigned long int NS_IN_MS = 1000000;
static const unsigned char CLOCK_PERIOD_NS = 125;
static const unsigned int US_IN_MS = 1000;


// prescale must be in [0, 7]
// 1 tick = 2**prescale us
void timer_init(unsigned int periodNs) {

    unsigned char prescale = 0;
    
    periodNs /= CLOCK_PERIOD_NS;
    while (periodNs > 1 && prescale < 8) {
        periodNs >>= 1;
        ++prescale;
    }   

    TSCR1 |= 0x80;
    TSCR2 &= 0xF8;
    TSCR2 |= prescale;
    TIOS |= 0x01;
    TCTL2 &= 0xFC;
    TCTL2 |= 0x01;
    TFLG1 |= 0x01;
}

// only use with 8us period
void sleep_ms(unsigned long int totalMs) {

    unsigned long int msElapsed;
    unsigned int counts = 125;

    TC0 = TCNT + counts;
    for (msElapsed = 1; msElapsed <= totalMs; ++msElapsed) {

        TFLG1 |= 0x01;
        while (!(TFLG1 & 0x01));
        TC0 += counts;
    }
}

unsigned int hex_to_bcd(unsigned int hex) {

    unsigned int bcd = 0;
    unsigned int remainderScale = 1;                        // Scale begins at 16^0
    
    if (hex < MAX_HEX_TO_BCD_ENCODE)
        while (hex) {
            bcd += (hex % DEC_BASE) * remainderScale;       // Scaled bottom digit added to bcd
            hex /= DEC_BASE;                                // Bottom digit removed from hex
            remainderScale <<= NIBBLE_SIZE;                 // Scale is increased: 16^i -> 16^(i+1)
        }
    
    return bcd;
}

unsigned int bcd_to_hex(unsigned int bcd) {

    unsigned int hex = 0;
    unsigned char count = 0;
    unsigned char currentDigit = 0;
    unsigned int remainderScale = 1;                    // Scale begins at 10^0
    
    while (count < NIBBLE_SIZE) {
        
        currentDigit = bcd % HEX_BASE;
        if (currentDigit < DEC_BASE) {
            hex += currentDigit * remainderScale;       // Scaled bottom digit added to hex
            bcd >>= NIBBLE_SIZE;                        // Bottom digit removed from bcd
            remainderScale *= DEC_BASE;                 // Scale is increased: 10^i -> 10^(i+1)
            ++count;
        } else {
            hex = 0;
            count = NIBBLE_SIZE;
        }
    }
    
    return hex;
}

unsigned char hex_to_ascii(unsigned char hex) {

    if (hex < 0x0A)
        hex += 0x30;
    else if (hex < 0x10)
        hex += 0x37;
    else
        hex = '?';
    return hex;
}

unsigned char ascii_to_hex(unsigned char ascii) {

    if (ascii > '/' && ascii < ':')
        ascii -= 0x30;
    else if (ascii > '@' && ascii < 'G')
        ascii -= 0x37;
    else if (ascii > '`' && ascii < 'g')
        ascii -= 0x57;
    else
        ascii = 0xFF;

    return ascii;
}

unsigned char to_upper(unsigned char ascii) {
    return is_alpha(ascii) ? ascii & 0xDF : ascii;
}

unsigned char to_lower(unsigned char ascii) {
    return is_alpha(ascii) ? ascii | 0x20 : ascii;
}

unsigned char is_alpha(unsigned char ascii) {
    return IS_CONSTRAINED(ascii, 0x41, 0x5A) | IS_CONSTRAINED(ascii, 0x60, 0x7B);
}

unsigned char is_numeric(unsigned char ascii, unsigned char base);

unsigned int get_number_of_digits_ulong(unsigned long int n, unsigned char base) {

    unsigned int digits = 0;
    base = CONSTRAIN(base, BIN_BASE, HEX_BASE);

    while (n) {
        n /= base;
        ++digits;
    }

    return digits ? digits : 1;
}

unsigned int get_number_of_digits_uint(unsigned int n, unsigned char base) {

    unsigned int digits = 0;
    base = CONSTRAIN(base, BIN_BASE, HEX_BASE);

    while (n) {
        n /= base;
        ++digits;
    }

    return digits ? digits : 1;
}

void uint_to_ascii(unsigned int n, unsigned char base, char* buffer, unsigned int bufferLength) {

    unsigned int digits;
    unsigned int currentPosition;

    digits = get_number_of_digits_uint(n, base);
    currentPosition = digits;

    if (digits < bufferLength) {
        while (n) {
            *(buffer + --currentPosition) = hex_to_ascii(n % base);
            n /= base;
        }

        while (digits < bufferLength)
            *(buffer + digits++) = 0;
    } else {
        fill_buffer(buffer, bufferLength, '?');
        *(buffer + bufferLength - 1) = 0;
    }
}

void ulong_to_ascii(unsigned long int n, unsigned char base, char* buffer, unsigned int bufferLength) {

    unsigned int digits;
    unsigned int currentPosition;

    digits = get_number_of_digits_ulong(n, base);
    currentPosition = digits;

    if (digits < bufferLength) {
        while (n) {
            *(buffer + --currentPosition) = hex_to_ascii(n % base);
            n /= base;
        }

        while (digits < bufferLength)
            *(buffer + digits++) = 0;
    } else {
        fill_buffer(buffer, bufferLength, '?');
        *(buffer + bufferLength - 1) = 0;
    }
}

void fill_buffer(char* buffer, unsigned int bufferLength, char c) {

    unsigned int i;
    for (i = 0; i < bufferLength; ++i)
        *(buffer + i) = c;
}

void copy_to_buffer(char* string, char* buffer, unsigned int bufferLength) {

    unsigned int i = 0;
    while (*(string + i) && i < bufferLength) {
        *(buffer + i) = *(string + i);
        ++i;
    }

    for (; i < bufferLength; ++i)
        *(buffer + i) = 0;   
}

void act_on_buffer(char* buffer, char (*fn)(char)) {

    unsigned int i = 0;
    while (*(buffer + i))
        *(buffer + i) = (*fn)(*(buffer + i++));
}