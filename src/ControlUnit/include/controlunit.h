#include <Arduino.h>

// typedef uint8_t byte;

#ifndef _BIT_POSITIONS
#define _BIT_POSITIONS
#define bit0(i) (((i) & 0x1) << 0)
#define bit1(i) (((i) & 0x1) << 1)
#define bit2(i) (((i) & 0x1) << 2)
#define bit3(i) (((i) & 0x1) << 3)
#define bit4(i) (((i) & 0x1) << 4)
#define bit5(i) (((i) & 0x1) << 5)
#define bit6(i) (((i) & 0x1) << 6)
#define bit7(i) (((i) & 0x1) << 7)
#endif

#include <controlunit_config.h>

void obd2_handle_request_frame(int address, byte add_bytes, byte *buf);

// Can bus
//================================================================================

#define CAN0_INT 2
#define CAN0_CLOCK_SELECT 10

typedef void (*can_frame_handler_func)(uint32_t canid, byte len, byte *buf);

typedef struct
{
    uint32_t canid_from;
    uint32_t canid_to;
    can_frame_handler_func func;
} can_frame_handler;

boolean can_init();
void can_print_frame(uint32_t id, size_t len, byte *buf, bool isOut);
void can0_send_frame(uint32_t id, size_t len, byte *buf);

can_frame_handler *can0_attach_handler(uint32_t canid_from_inclusive, uint32_t canid_to_inclusive,
                                       can_frame_handler_func func);

// OBD-II
//================================================================================

void obd2_handle_request_frame(int address, byte add_bytes, byte *buf);