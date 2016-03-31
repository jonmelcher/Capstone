// *********************************************
//  Filename:       atd0_lib.h
//  Purpose:        Header file for ATD0 Library
//  Dependencies:   <hidef.h>, "derivative.h"
//  Processor:      MC9S12XDP512
//  Crystal:        16 MHz
//  Author:         Jonathan Melcher
//  Last Updated:   12/12/2015
// *********************************************

#ifndef _HIDEF_H_
#define _HIDEF_H_
#include <hidef.h>
#endif

#ifndef _DERIVATIVE_H_
#define _DERIVATIVE_H_
#include "derivative.h"
#endif

#ifndef _ATD0_LIB_H_
#define _ATD0_LIB_H_

void atd0_init(void);
unsigned int atd0_read(void);

#endif