// **********************************************************
//  Filename:       common.h
//  Purpose:        Header file containing common definitions
//  Processor:      MC9S12XDP512
//  Crystal:        16 MHz
//  Author:         Jonathan Melcher
//  Last Updated:   20/11/2015
// **********************************************************

#ifndef _HIDEF_H_
#define _HIDEF_H_
#include <hidef.h>
#endif

#ifndef _DERIVATIVE_H_
#define _DERIVATIVE_H_
#include "derivative.h"
#endif

#ifndef _COMMON_H_
#define _COMMON_H_

static const unsigned char NIBBLE_SIZE = 0x04;
static const unsigned char BYTE_SIZE = 0x08;
static const unsigned char BIN_BASE = 0x02;
static const unsigned char OCT_BASE = 0x08;
static const unsigned char DEC_BASE = 0x0A;
static const unsigned char HEX_BASE = 0x10;

#endif