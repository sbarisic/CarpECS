#include <controlunit.h>

#include <SPI.h>
#include <mcp_can.h>

// Set INT to pin 2
MCP_CAN CAN0(CAN0_CLOCK_SELECT);

unsigned long canId = 0;
byte len = 0;
byte buf[8];

//================================================================================

#define MAX_CAN_HANDLERS 16

can_frame_handler can_handlers[MAX_CAN_HANDLERS];
byte can_handlers_count = 0;

//================================================================================

bool lastIsOut = true;

void can_print_frame(uint32_t id, size_t len, byte *buf, bool isOut)
{
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

void can0_receive()
{
    if (CAN0.readMsgBuf(&canId, &len, buf) == CAN_OK)
    {
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
}

boolean can_init()
{
    pinMode(CAN0_INT, INPUT_PULLUP);
    attachInterrupt(digitalPinToInterrupt(CAN0_INT), can0_receive, FALLING);

    if (CAN0.begin(MCP_STDEXT, CAN_500KBPS, MCP_16MHZ) == CAN_OK)
    {
        CAN0.setMode(MCP_NORMAL);
        return true;
    }

    return false;
}