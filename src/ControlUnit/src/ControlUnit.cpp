#define CONTROLUNIT_IMPL
#include <controlunit.h>
#undef CONTROLUNIT_IMPL

void setup()
{
    Serial.begin(115200);

    while (!can_init())
    {
        printf("CAN init failed");
        delay(1000);
    }

    printf("CAN init ok");

    can0_attach_handler(0x7DF, 0x7DF, [](uint32_t canid, byte len, byte *buf) -> void {
        obd2_handle_request_frame(canid, buf[0], buf + 1);
    });
}

void loop()
{
}
