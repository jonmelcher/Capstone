// ********************************************
//  Filename:       lcd_lib.h
//  Purpose:        Header file for LCD library
//  Dependencies:   <hidef.h>, "derivative.h"
//  Processor:      MC9S12XDP512
//  Crystal:        16 MHz
//  Author:         Jonathan Melcher
//  Last Updated:   20/11/2015
// ********************************************

#ifndef _HIDEF_H_
#define _HIDEF_H_
#include <hidef.h>
#endif

#ifndef _DERIVATIVE_H_
#define _DERIVATIVE_H_
#include "derivative.h"
#endif

#ifndef _LCD_LIB_H_
#define _LCD_LIB_H_

void lcd_init(void);
void lcd_tx_control(unsigned char control);
unsigned char lcd_busy(void);
void lcd_tx_char(char character);
void lcd_clear(void);
void lcd_set_cursor_at_address(unsigned char address);
void lcd_set_cursor_at_row_col(unsigned char row, unsigned char col);
void lcd_tx_string(char* string);

#endif