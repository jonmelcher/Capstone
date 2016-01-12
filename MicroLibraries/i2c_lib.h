// ********************************************
//  Filename:       i2c_lib.h
//  Purpose:        Header file for I2C Library
//  Dependencies:   <hidef.h>, "derivative.h"
//  Processor:      MC9S12XDP512
//  Crystal:        16 MHz
//  Author:         Jonathan Melcher
//  Last Updated:   12/12/2015
// ********************************************

#ifndef _HIDEF_H_
#define _HIDEF_H_
#include <hidef.h>
#endif

#ifndef _DERIVATIVE_H_
#define _DERIVATIVE_H_
#include "derivative.h"
#endif

#ifndef _I2C_LIB_H_
#define _I2C_LIB_H_

void iic0_init(void);
void iic0_transmit(unsigned char val);
void iic0_write(unsigned char addr, unsigned char reg, unsigned char val);
unsigned char iic0_read(unsigned char addr, unsigned char reg);

#endif