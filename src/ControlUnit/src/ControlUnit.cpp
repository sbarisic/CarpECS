#define CONTROLUNIT_IMPL
#include <controlunit.h>
#undef CONTROLUNIT_IMPL

#include <ecumaster.h>

emu_data_t emu_data;

void setup()
{
    Serial.begin(115200);

    while (!can_init())
    {
        printf("CAN init failed");
        delay(1000);
    }
    printf("CAN init ok");

    can0_attach_handler(0x7DF, 0x7E0, [](uint32_t canid, byte len, byte *buf) -> void {
        obd2_handle_request_frame(canid, buf[0], buf + 1);
    });

    can0_attach_handler(0x600, 0x600, [](uint32_t canid, byte len, byte *buf) -> void {
        ecumaster_handle_frame(canid, len, buf, &emu_data);
    });
}

void loop()
{
}
