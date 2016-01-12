// ******************************************
//  Filename:       i2c_lib.c
//  Purpose:        Source file for i2c_lib.h
//  Dependencies:   <hidef.h>, "derivative.h"
//  Processor:      MC9S12XDP512
//  Crystal:        16 MHz
//  Author:         Jonathan Melcher
//  Last Updated:   12/12/2015
// ******************************************

#include "i2c_lib.h"


// Communication rate settings: 100kHz, SDA Hold = 20 cks, SCL Hold Start = 32 SCL Hold Stop = 42
static const unsigned char COMMUNICATION_RATE_SETTINGS = 0x47;
static const unsigned char STOP_TRANSMITTING_EXIT_MASTER = 0xCF;
static const unsigned char STOP_RECEIVING_EXIT_MASTER = 0xDF;
static const unsigned char DEFAULT_MASTER_ADDRESS = 0x30;
static const unsigned char TX_RX_FLAG = 0x02;
static const unsigned char BUSY_FLAG = 0x20;

void iic0_init(void) {

    IIC0_IBFD = COMMUNICATION_RATE_SETTINGS;            // set up communication rate
    IIC0_IBCR |= 0x80;                                  // enable the bus
    IIC0_IBCR &= 0xBE;                                  // no interrupts, normal WAI
}
  
void iic0_write(unsigned char addr, unsigned char reg, unsigned char val) {

    while(IIC0_IBSR & BUSY_FLAG);                     // wait for not busy flag

    IIC0_IBCR |= DEFAULT_MASTER_ADDRESS;              // set master
    iic0_transmit(addr & 0xFE);                       // transmit bitmasked address
    iic0_transmit(reg);                               // transmit register
    iic0_transmit(val);                               // transmit value
    IIC0_IBCR &= STOP_TRANSMITTING_EXIT_MASTER;       // stop transmission
}

void iic0_transmit(unsigned char val) {

    IIC0_IBDR = val;                                  // copy value to register
    while(!(IIC0_IBSR & TX_RX_FLAG));                 // wait for flag
    IIC0_IBSR |= TX_RX_FLAG;                          // clear flag
}

unsigned char iic0_read(unsigned char addr, unsigned char reg) {

    unsigned char val;
  
    while(IIC0_IBSR & BUSY_FLAG);                     // wait for not busy flag

    IIC0_IBCR |= DEFAULT_MASTER_ADDRESS;              // set master
    iic0_transmit(addr & 0xFE);                       // transmit bitmasked address
    iic0_transmit(reg);                               // transmit register
  
    IIC0_IBCR |= 0x04;                                // restart

    iic0_transmit(addr | 0x01);                       // transmit address with read
  
    IIC0_IBCR |= 0x08;                                // reading 1 unsigned char only
    IIC0_IBCR &= 0xEF;                                // receive unsigned char
    val = IIC0_IBDR;                                  // starts reception process

    while(!(IIC0_IBSR & TX_RX_FLAG));                 // wait for flag
    IIC0_IBCR &= STOP_RECEIVING_EXIT_MASTER;          // stop reception
    IIC0_IBSR |= TX_RX_FLAG;                          // clear flag
  
    val = IIC0_IBDR;                                  // retrieve value
    return val;
}