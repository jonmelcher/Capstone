// ******************************************************
//  Filename:       misc_lib.h
//  Purpose:        Header file for misc library
//  Dependencies:   <hidef.h>, "derivative.h", "common.h"
//  Processor:      MC9S12XDP512
//  Crystal:        16 MHz
//  Author:         Jonathan Melcher
//  Last Updated:   20/11/2015
// ******************************************************

#ifndef _HIDEF_H_
#define _HIDEF_H_
#include <hidef.h>
#endif

#ifndef _DERIVATIVE_H_
#define _DERIVATIVE_H_
#include "derivative.h"
#endif

#ifndef _MISC_LIB_H_
#define _MISC_LIB_H_

#include "common.h"

#define CONSTRAIN(n, lower, upper)((n) < (lower) ? (lower) : (n) > (upper) ? (upper) : (n))
#define IS_CONSTRAINED(n, lower, upper)((n) > (lower) && (n) < (upper) ? 1 : 0)
#define MINIMUM(n, m)((n) < (m) ? (n) : (m))
#define MAXIMUM(n, m)((n) > (m) ? (n) : (m))

void timer_init(unsigned int periodNs);
void sleep_ms(unsigned long int totalMs);

unsigned int hex_to_bcd(unsigned int hex);
unsigned int bcd_to_hex(unsigned int bcd);
unsigned char hex_to_ascii(unsigned char hex);
unsigned char ascii_to_hex(unsigned char ascii);

unsigned char to_upper(unsigned char ascii);
unsigned char to_lower(unsigned char ascii);
unsigned char is_alpha(unsigned char ascii);
unsigned char is_numeric(unsigned char ascii, unsigned char base);

unsigned int get_number_of_digits_ulong(unsigned long int n, unsigned char base);
unsigned int get_number_of_digits_uint(unsigned int n, unsigned char base);
void uint_to_ascii(unsigned int n, unsigned char base, char* buffer, unsigned int bufferLength);
void ulong_to_ascii(unsigned long int n, unsigned char base, char* buffer, unsigned int bufferLength);

void fill_buffer(char* buffer, unsigned int bufferLength, char c);
void copy_to_buffer(char* string, char* buffer, unsigned int bufferLength);
void act_on_buffer(char* buffer, char (*fn)(char));

#endif