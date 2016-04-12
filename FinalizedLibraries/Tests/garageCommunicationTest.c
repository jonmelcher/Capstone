#include "garageCommunication.h"

static const unsigned char START_INSTRUCTION = 0xF0;
static const unsigned char HOME = 0;
static const unsigned char CELLS_PER_TIER = 8;
static const unsigned long int DEGREES_BETWEEN_CELLS = 45;
static const unsigned char OUTGOING = 0x20;
static const unsigned char INCOMING = 0x21;
static const unsigned long long int DELAY_BETWEEN_MOVEMENTS_MS = 1000;
static const unsigned char CONTINUE_INSTRUCTION = 0xF1;
static const unsigned char STOP_INSTRUCTION = 0xF2;
static const unsigned char INSTRUCTIONS_COMPLETED = 0xF3;

void main(void) {
    unsigned char cell;
    unsigned char direction;
    _DISABLE_COP();
    garage_communication_init();

    for(;;) {
        while (garage_rx() != START_INSTRUCTION);
        garage_tx(CONTINUE_INSTRUCTION);
        do
            cell = garage_rx();
        while (!cell || cell >= 24);
        garage_tx(CONTINUE_INSTRUCTION);
        do
            direction = garage_rx();
        while (direction != OUTGOING && direction != INCOMING);
        garage_tx(CONTINUE_INSTRUCTION);
        while (garage_rx() != STOP_INSTRUCTION);
        garage_tx(INSTRUCTIONS_COMPLETED);
    }
}