// ********************************************
//  Filename:       lcd_lib.c
//  Purpose:        Source file for LCD library
//  Processor:      MC9S12XDP512
//  Crystal:        16 MHz
//  Author:         Jonathan Melcher
//  Last Updated:   20/11/2015
// ********************************************


#include "lcd_lib.h"


static const unsigned char ROW_MAX = 4;
static const unsigned char COL_MAX = 20;
static const unsigned char REST_STATE = 0xF8;
static const unsigned char BUSY_STATE = 0x80;
static const unsigned char READ = 0x03;
static const unsigned char CONTROL_WRITE = 0x01;
static const unsigned char CHARACTER_WRITE = 0x05;


void lcd_init(void) {

    PTH = 0x00;                 // clear register
    DDRH = 0xFF;                // data bus as all outputs for writing
    PORTK &= REST_STATE;        // preset RS LOW, R/W LOW, EN LOW (11111000)
    DDRK |= 0x07;               // activate three control lines (00000111)

    // we require a delay period of longer than 15 ms: the following is about 49ms
    asm PSHD;
    asm LDD #0;
    asm DBNE D, *;      // 24.576 ms delay
    asm DBNE D, *;      // ... twice
    asm PULD;

    // default setup: 8-bit, 2 lines, 5x8 character matrix
    PTH = 0x38;
    PORTK |= CONTROL_WRITE;
    PORTK &= REST_STATE;

    // we require a delay period of more than 4.1ms: the following is about 4.125ms
    asm PSHD;
    asm LDD #11000;
    asm DBNE D, *;
    asm PULD;

    PORTK |= CONTROL_WRITE;
    PORTK &= REST_STATE;

    // we require a delay period of more than 100us: the following is about 100us
    asm PSHD;
    asm LDD #267;
    asm DBNE D, *;
    asm PULD;

    lcd_tx_control(0x38);       // default setup: 8-bit, 2 lines, 5x8 character matrix
    lcd_tx_control(0x0E);       // control for cursor settings: display on, cursor on, blink off
    lcd_tx_control(0x06);       // control for cursor settings: increments to the left, no shift
    lcd_clear();
}

void lcd_tx_control(unsigned char control) {

    while (lcd_busy());

    PTH = control;
    PORTK |= CONTROL_WRITE;
    PORTK &= REST_STATE;
}

unsigned char lcd_busy(void) {

    unsigned char busy;

    DDRH = 0x00;
    PORTK |= READ;
    PORTK &= REST_STATE;

    busy = PTH & BUSY_STATE;
    DDRH = 0xFF;

    return busy;
}

void lcd_tx_char(char character) {

    while (lcd_busy());

    PTH = character;
    PORTK |= CHARACTER_WRITE;
    PORTK &= REST_STATE;
}

void lcd_clear(void) {
    lcd_tx_control(0x01);
}

void lcd_set_cursor_at_address(unsigned char address) {
    lcd_tx_control(address | 0x80);
}

void lcd_set_cursor_at_row_col(unsigned char row, unsigned char col) {

    unsigned char address = 0;
    unsigned char smallShift = 0x14;
    unsigned char largeShift = 0x2C;

    if (row < ROW_MAX && col < COL_MAX) {

        switch (row) {
            case 3:
                address += smallShift;
            case 1:
                address += largeShift;
            case 2:
                address += smallShift;
            default:
                address += col;
                break;
        }
    }

    lcd_set_cursor_at_address(address);
}

void lcd_tx_string(char* string) {

    int i = 0;
    while (*(string + i)) 
        lcd_tx_char(*(string + i++));
}