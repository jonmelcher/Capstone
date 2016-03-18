// ****************************************************************
//  filename    :   timer.h
//  purpose     :   header for timer.c
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016/03/16
// ****************************************************************

#ifndef _HIDEF_H_
#define _HIDEF_H_
#include <hidef.h>
#endif

#ifndef _DERIVATIVE_H_
#define _DERIVATIVE_H_
#include "derivative.h"
#endif

#ifndef _TIMER_H_
#define _TIMER_H_

void timer_init_8us(void);
void timer_delay_ms(unsigned long long int ms);

#endif