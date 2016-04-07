// ****************************************************************
//  filename    :   garageCommunication.c
//  purpose     :   facilitate serial communication with garage
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016-03-24
// ****************************************************************


#include "garageCommunication.h"


static const unsigned char RX_FULL_MASK = 0x20;
static const unsigned char TX_EMPTY_MASK = 0x80;


void garage_communication_init(void) {
    SCI0BD = 26;               // sets baud-rate to 19200
    SCI0CR1 = 0;                                    // wait-mode enabled, no loopback, 8-bit data
    SCI0CR2 = 0x0C;                                 // wake-up on start, no parity checking, TX/RX enabled, no interrupts
}

unsigned char is_garage_rx_ready(void) {
    return SCI0SR1 & RX_FULL_MASK;                  // check if RX_FULL flag is high
}

unsigned char garage_rx(void) {
    unsigned char rxed;
    while (!is_garage_rx_ready());                  // wait until datum is received
    rxed = SCI0DRL;                                 // retrieve datum
    return rxed;
}

unsigned char is_garage_tx_ready(void) {
    return SCI0SR1 & TX_EMPTY_MASK;                 // check if TX_EMPTY flag is high
}

void garage_tx(unsigned char datum) {
    while (!is_garage_tx_ready()) ;                 // wait until ready to transmit
    SCI0DRL = datum;                                // transmit datum
}