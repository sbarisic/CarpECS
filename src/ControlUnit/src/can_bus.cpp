#include <controlunit.h>

#include <SPI.h>
#include <mcp_can.h>

// Set INT to pin 2
MCP_CAN CAN0(CAN0_CLOCK_SELECT);

unsigned long canId = 0;
byte len = 0;
byte buf[8];

volatile boolean can0_interrupted = false;

//================================================================================

#define MAX_CAN_HANDLERS 16

can_frame_handler can_handlers[MAX_CAN_HANDLERS];
byte can_handlers_count = 0;

//================================================================================

bool lastIsOut = true;

void can_print_frame(uint32_t id, size_t len, byte *buf, bool isOut)
{
#ifndef ENABLE_SERIAL_PRINT
    return;
#endif

    // Temp buffer
    char buffer[6];

    if (lastIsOut != isOut)
    {
        if (isOut)
        {
            Serial.println("OUT:");
        }
        else
        {
            Serial.println("IN:");
        }
    }

    lastIsOut = isOut;

    Serial.print("    ID: 0x");
    Serial.print(id, HEX);
    Serial.print("; DLC: 0x");
    Serial.print(len, HEX);
    Serial.print("; DATA:");

    for (int i = 0; i < 8; i++)
    {
        // Serial.print(" 0x");

        sprintf(buffer, " %02x", buf[i]);
        Serial.print(buffer);
    }

    Serial.println();
}

void can0_send_frame(uint32_t id, size_t len, byte *buf)
{
    can_print_frame(id, len, buf, true);
    CAN0.sendMsgBuf(id, len, buf);
}

can_frame_handler *can0_attach_handler(uint32_t canid_from_inclusive, uint32_t canid_to_inclusive,
                                       can_frame_handler_func func)
{
    if (can_handlers_count >= MAX_CAN_HANDLERS)
        return NULL;

    byte idx = can_handlers_count++;
    can_handlers[idx].canid_from = canid_from_inclusive;
    can_handlers[idx].canid_to = canid_to_inclusive;
    can_handlers[idx].func = func;

    return &can_handlers[idx];
}

boolean skip_canid(unsigned long canId)
{
    switch (canId)
    {
    case 0x120:
    case 0x12A:
    case 0x135:
    case 0x137:
    case 0x139:
    case 0x140:
    case 0x148:
    case 0x160:
    case 0x17D:
    case 0x182:
    case 0x185:
    case 0x1C6:
    case 0x1C7:
    case 0x1C8:
    case 0x1CE:
    case 0x1E1:
    case 0x1E5:
    case 0x1E9:
    case 0x1F1:
    case 0x1F3:
    case 0x210:
    case 0x214:
    case 0x232:
    case 0x234:
    case 0x2F1:
    case 0x2F9:
    case 0x32A:
    case 0x348:
    case 0x34A:
    case 0x362:
    case 0x365:
    case 0x370:
    case 0x3C9:
    case 0x3CB:
    case 0x3E7:
    case 0x3F1:
    case 0x451:
    case 0x4C5:
    case 0x4D7:
    case 0x4E1:
    case 0x4E9:
    case 0x500:
    case 0x514:
    case 0x52A:
    case 0x530:
    case 0x773:
    case 0x780:
    case 0xC1:
    case 0xC5:
    case 0xD1:
    case 0xF1:
        return true;
    }

    return false;
}

void can_loop()
{
    // if (can0_interrupted)
    //{
    //     can0_interrupted = false;

    if (CAN0.readMsgBuf(&canId, &len, buf) == CAN_OK)
    {
        if (skip_canid(canId))
            return;

        can_print_frame(canId, len, buf, false);

        for (size_t i = 0; i < can_handlers_count; i++)
        {
            if (can_handlers[i].canid_from <= canId && can_handlers[i].canid_to >= canId)
            {
                can_frame_handler_func f = can_handlers[i].func;

                if (f != NULL)
                    f(canId, len, buf);
            }
        }
    }
    //}
}

void can0_receive()
{
    can0_interrupted = true;
}

boolean can_init()
{
    pinMode(CAN0_INT, INPUT_PULLUP);
    // attachInterrupt(digitalPinToInterrupt(CAN0_INT), can0_receive, FALLING);

    if (CAN0.begin(MCP_STDEXT, CAN_500KBPS, MCP_16MHZ) == CAN_OK)
    {
        CAN0.setMode(MCP_NORMAL);
        return true;
    }

    return false;
}