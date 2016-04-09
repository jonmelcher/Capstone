// ****************************************************************
//  filename    :   garageCommunication.h
//  purpose     :   header file for garageCommunication.c
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016-03-24
// ****************************************************************

#ifndef _HIDEF_H_
#define _HIDEF_H_
#include <hidef.h>
#endif

#ifndef _DERIVATIVE_H_
#define _DERIVATIVE_H_
#include "derivative.h"
#endif

#ifndef _GARAGE_COMMUNICATION_H_
#define _GARAGE_COMMUNICATION_H_

void garage_communication_init(void);
unsigned char is_garage_rx_ready(void);
unsigned char garage_rx(void);
unsigned char is_garage_tx_ready(void);
void garage_tx(unsigned char datum);

#endif