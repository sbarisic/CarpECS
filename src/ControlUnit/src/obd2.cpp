#include <controlunit.h>

// Default reply ECU ID
#define REPLY_ID 0x7E8

typedef void (*pid_handler)(byte pid, byte service_mode, byte add_bytes, byte *buf);

typedef struct
{
    int PID;
    pid_handler Func;
} service_01_pid;

byte tmp8[8] = {0};

#define DECLARE_SERVICE_01_PID_FUNC(FuncName) void FuncName(byte pid, byte service_mode, byte add_bytes, byte *buf)

DECLARE_SERVICE_01_PID_FUNC(obd2_service_01_pid_00_20_40_60_80_A0);
//DECLARE_SERVICE_01_PID_FUNC(obd2_service_01_pid_05);
DECLARE_SERVICE_01_PID_FUNC(obd2_service_01_pid_1C);
//DECLARE_SERVICE_01_PID_FUNC(obd2_service_01_pid_1F);
//DECLARE_SERVICE_01_PID_FUNC(obd2_service_01_pid_0D);
DECLARE_SERVICE_01_PID_FUNC(obd2_service_01_pid_51);
//DECLARE_SERVICE_01_PID_FUNC(obd2_service_01_pid_0C);
DECLARE_SERVICE_01_PID_FUNC(obd2_service_01_pid_01_41);

service_01_pid obd2_service_01_PIDs[] = {{0x00, obd2_service_01_pid_00_20_40_60_80_A0},
                                         {0x20, obd2_service_01_pid_00_20_40_60_80_A0},
                                         {0x40, obd2_service_01_pid_00_20_40_60_80_A0},
                                         {0x60, obd2_service_01_pid_00_20_40_60_80_A0},
                                         {0x80, obd2_service_01_pid_00_20_40_60_80_A0},
                                         {0xA0, obd2_service_01_pid_00_20_40_60_80_A0},

                                         //{0x05, obd2_service_01_pid_05},
                                         {0x1C, obd2_service_01_pid_1C},
                                         //{0x1F, obd2_service_01_pid_1F},
                                         //{0x0D, obd2_service_01_pid_0D},
                                         {0x51, obd2_service_01_pid_51},
                                         //{0x0C, obd2_service_01_pid_0C},
                                         {0x01, obd2_service_01_pid_01_41},
                                         {0x41, obd2_service_01_pid_01_41}};

size_t obd2_get_service_01_count()
{
    return (sizeof(obd2_service_01_PIDs) / sizeof(*obd2_service_01_PIDs));
}

int obd2_get_service_01_pid_pid(int i)
{
    return obd2_service_01_PIDs[i].PID;
}

void obd2_handle_service_01(int add_bytes, byte pid, byte *buf)
{
    const int service_mode = 0x1;

    for (size_t i = 0; i < obd2_get_service_01_count(); i++)
    {
        if (obd2_service_01_PIDs[i].PID == pid)
        {
            obd2_service_01_PIDs[i].Func(pid, service_mode, add_bytes, buf);
            return;
        }
    }
}

void obd2_create_response_frame_mode06(byte *out, byte service_mode, byte tid, byte limit_type, byte A, byte B, byte C,
                                       byte D)
{
    out[0] = 7;
    out[1] = service_mode + 0x40;
    out[2] = tid;
    out[3] = limit_type;
    out[4] = A;
    out[5] = B;
    out[6] = C;
    out[7] = D;
}

void obd2_create_response_frame(byte *out, byte service_mode, byte pid, size_t len, byte *bytes)
{
    for (size_t i = 0; i < 8; i++)
        out[i] = 0;

    int offset = 0;
    out[offset++] = len; // Length
    out[offset++] = service_mode + 0x40;

    if (pid >= 0)
        out[offset++] = pid;

    out[0] = out[0] + offset;

    for (size_t i = 0; i < len; i++)
        out[i + offset] = bytes[i];
}

void obd2_create_response_frame(byte *out, int service_mode, int pid)
{
    obd2_create_response_frame(out, service_mode, pid, 0, NULL);
}

void obd2_create_response_frame(byte *out, int service_mode, int pid, uint32_t u32)
{
    byte *bytes = (byte *)&u32;
    byte obd_bytes[] = {bytes[3], bytes[2], bytes[1], bytes[0]};
    obd2_create_response_frame(out, service_mode, pid, 4, obd_bytes);
}

void obd2_send_frame(uint32_t id, size_t len, byte *buf)
{
    can0_send_frame(id, len, buf);
}

//=======================================================================================
// SERVICE 01 PIDs
//=======================================================================================

/*// Run time since engine start
void obd2_service_01_pid_1F(byte pid, byte service_mode, byte add_bytes, byte *buf)
{
    byte dat[2] = {0x00, 0x3C};
    obd2_create_response_frame(tmp8, service_mode, pid, sizeof(dat) / sizeof(*dat), dat);
    obd2_send_frame(REPLY_ID, 8, tmp8);
}

// RPM
void obd2_service_01_pid_0C(byte pid, byte service_mode, byte add_bytes, byte *buf)
{
    byte dat[2] = {0x25, 0x25};
    obd2_create_response_frame(tmp8, service_mode, pid, sizeof(dat) / sizeof(*dat), dat);
    obd2_send_frame(REPLY_ID, 8, tmp8);
}

// Vehicle speed
void obd2_service_01_pid_0D(byte pid, byte service_mode, byte add_bytes, byte *buf)
{
    byte dat[1] = {100};
    obd2_create_response_frame(tmp8, service_mode, pid, sizeof(dat) / sizeof(*dat), dat);
    obd2_send_frame(REPLY_ID, 8, tmp8);
}*/

// OBD standards this vehicle conforms to
void obd2_service_01_pid_1C(byte pid, byte service_mode, byte add_bytes, byte *buf)
{
    byte dat[1] = {obd_Std};
    obd2_create_response_frame(tmp8, service_mode, pid, sizeof(dat) / sizeof(*dat), dat);
    obd2_send_frame(REPLY_ID, 8, tmp8);
}

// Fuel type
void obd2_service_01_pid_51(byte pid, byte service_mode, byte add_bytes, byte *buf)
{
    byte dat[1] = {fuel_Type};
    obd2_create_response_frame(tmp8, service_mode, pid, sizeof(dat) / sizeof(*dat), dat);
    obd2_send_frame(REPLY_ID, 8, tmp8);
}

/*// ECT
void obd2_service_01_pid_05(byte pid, byte service_mode, byte add_bytes, byte *buf)
{
    byte dat[1] = {(byte)(95 + 40)};
    obd2_create_response_frame(tmp8, service_mode, pid, sizeof(dat) / sizeof(*dat), dat);
    obd2_send_frame(REPLY_ID, 8, tmp8);
}*/

void obd2_service_01_pid_01_41(byte pid, byte service_mode, byte add_bytes, byte *buf)
{
    // 0x01 - Monitor status since DTCs cleared
    // 0x41 - Monitor status this drive cycle

    // Note that for bits indicating test availability a bit set to 1 indicates available
    // whilst for bits indicating test completeness a bit set to 0 indicates complete.

    byte A = bit7(MIL ? 1 : 0) | (CEL_Codes & 0b1111111);
    byte B = bit6(0) | bit5(0) | bit4(0) | bit3(0) | bit2(1) | bit1(1) | bit0(1);
    byte C = bit7(1) | bit6(1) | bit5(1) | bit4(0) | bit3(0) | bit2(0) | bit1(0) | bit0(1); // Availability
    byte D = bit7(0) | bit6(0) | bit5(0) | bit4(0) | bit3(0) | bit2(0) | bit1(0) | bit0(0); // Completeness

    byte bit2 = bit6(0) | bit5(0) | bit4(0) | bit3(0) | bit2(1) | bit1(0) | bit0(1);
    byte C2 = bit7(1) | bit6(1) | bit5(1) | bit4(0) | bit3(0) | bit2(0) | bit1(0) | bit0(1); // Availability
    byte D2 = bit7(0) | bit6(0) | bit5(1) | bit4(0) | bit3(0) | bit2(0) | bit1(0) | bit0(1); // Completeness

    Serial.println("######### Emissions test " + String(pid));
    byte ABCD[4];

    if (pid == 0x1)
    {
        ABCD[0] = A;
        ABCD[1] = B;
        ABCD[2] = C;
        ABCD[3] = D;
    }
    else
    {
        ABCD[0] = 0;
        ABCD[1] = bit2;
        ABCD[2] = C2;
        ABCD[3] = D2;
    }

    obd2_create_response_frame(tmp8, service_mode, pid, 4, ABCD);
    obd2_send_frame(REPLY_ID, 8, tmp8);
}

// Show PIDs supported
void obd2_service_01_pid_00_20_40_60_80_A0(byte pid, byte service_mode, byte add_bytes, byte *buf)
{
    uint32_t supported = 0;
    size_t incl_from = pid + 0x01;
    size_t incl_to = incl_from + 0x1F;

    for (size_t i = 0; i < obd2_get_service_01_count(); i++)
    {
        uint32_t f_pid = obd2_get_service_01_pid_pid(i);

        if (f_pid >= incl_from && f_pid <= incl_to)
            supported = supported | ((uint32_t)1 << (32 - (f_pid - pid)));
    }

    obd2_create_response_frame(tmp8, service_mode, pid, supported);
    obd2_send_frame(REPLY_ID, 8, tmp8);
}

void obd2_handle_service_06(int add_bytes, int pid, byte *buf)
{
    Serial.println("############# SERVICE 0x06 #############");

    const int service_mode = 0x6;

    /*if (pid == 0x01)
    {
        obd2_create_response_frame_mode06(tmp8, service_mode, pid, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF);
        obd2_send_frame(REPLY_ID, 8, tmp8);
        delay(50);
    }
    else*/
    if (pid >= 0x1 && pid <= 0x20)
    {
        // byte arr[4] = {0xFF, 0xAB, 0xCD, 0xEF};
        // obd2_create_response_frame(tmp8, service_mode, pid, 4, arr);

        // obd2_create_response_frame_mode06(tmp8, service_mode, pid, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF);
        // obd2_send_frame(REPLY_ID, 8, tmp8);

        // obd2_create_response_frame_mode06(tmp8, service_mode, pid, 0xFF, 0xFF, 0x00, 0x00, 0x00);
        // obd2_send_frame(REPLY_ID, 8, tmp8);

        byte i = 0x0C;
        obd2_create_response_frame_mode06(tmp8, service_mode, pid, bit7(1) | ((byte)i), 0x00, 0x10, 0x00, 0x00);
        obd2_send_frame(REPLY_ID, 8, tmp8);
        delay(50);

        obd2_create_response_frame_mode06(tmp8, service_mode, pid, bit7(0) | ((byte)i), 0x00, 0x32, 0x00, 0x20);
        obd2_send_frame(REPLY_ID, 8, tmp8);
        delay(50);

        /*obd2_create_response_frame_mode06(tmp8, service_mode, pid, 0x16, 0x00, 0x32, 0x00, 0x20);
        obd2_send_frame(REPLY_ID, 8, tmp8);*/
    }
}

void obd2_handle_service_09(byte add_bytes, byte pid, byte *buf)
{
    const byte service_mode = 0x9;

    if (pid == 0x0) // Service 9 supported PIDs
    {
        byte A = bit7(0) | bit6(1) | bit5(0) | bit4(1) | bit3(0) | bit2(1) | bit1(0) | bit0(0);
        byte B = bit7(0) | bit6(1) | bit5(0) | bit4(0) | bit3(0) | bit2(0) | bit1(0) | bit0(0);
        byte C = bit7(0) | bit6(0) | bit5(0) | bit4(0) | bit3(0) | bit2(0) | bit1(0) | bit0(0);
        byte D = bit7(0) | bit6(0) | bit5(0) | bit4(0) | bit3(0) | bit2(0) | bit1(0) |
                 bit0(0); // bit7(0) | bit6(0) | bit5(0) | bit4(0) | bit3(0) | bit2(0) | bit1(0) | bit0(0);
        byte ABCD[4] = {A, B, C, D};

        obd2_create_response_frame(tmp8, service_mode, pid, 4, ABCD);
        obd2_send_frame(REPLY_ID, 8, tmp8);
    }
    else if (pid == 0x2) // VIN
    {
        byte frame1[8] = {0x10, 2 + 18, 0x49, pid, 1, vehicle_Vin[0], vehicle_Vin[1], vehicle_Vin[2]};
        byte frame2[8] = {0x21, vehicle_Vin[3], vehicle_Vin[4], vehicle_Vin[5],
                          vehicle_Vin[6], vehicle_Vin[7], vehicle_Vin[8], vehicle_Vin[9]};
        byte frame3[8] = {0x22,
                          vehicle_Vin[10],
                          vehicle_Vin[11],
                          vehicle_Vin[12],
                          vehicle_Vin[13],
                          vehicle_Vin[14],
                          vehicle_Vin[15],
                          vehicle_Vin[16]};

        obd2_send_frame(REPLY_ID, 8, frame1);
        obd2_send_frame(REPLY_ID, 8, frame2);
        obd2_send_frame(REPLY_ID, 8, frame3);
    }
    else if (pid == 0x4) // Cal ID
    {
        unsigned char frame1[8] = {0x10, 2 + 18, 0x49, pid, 1, calibration_ID[0], calibration_ID[1], calibration_ID[2]};
        unsigned char frame2[8] = {0x21,
                                   calibration_ID[3],
                                   calibration_ID[4],
                                   calibration_ID[5],
                                   calibration_ID[6],
                                   calibration_ID[7],
                                   calibration_ID[8],
                                   calibration_ID[9]};
        unsigned char frame3[8] = {0x22,
                                   calibration_ID[10],
                                   calibration_ID[11],
                                   calibration_ID[12],
                                   calibration_ID[13],
                                   calibration_ID[14],
                                   calibration_ID[15],
                                   calibration_ID[16]};

        obd2_send_frame(REPLY_ID, 8, frame1);
        obd2_send_frame(REPLY_ID, 8, frame2);
        obd2_send_frame(REPLY_ID, 8, frame3);
    }
    else if (pid == 0x6) // Calibration verification numbers
    {
        unsigned char frame1[8] = {0x10, 2 + (4 + 7 + 7 + 7 + 7), 0x49, pid, 0x08, 0xAB, 0xCD, 0xEF};
        unsigned char frame2[8] = {0x21, 0x00, 0x00, 0x00, 0xDE, 0xAD, 0xBE, 0xEF};
        unsigned char frame3[8] = {0x22, 0x00, 0x00, 0x00, 0xDE, 0xAD, 0xBE, 0xEF};
        unsigned char frame4[8] = {0x23, 0x00, 0x00, 0x00, 0xDE, 0xAD, 0xBE, 0xEF};
        unsigned char frame5[8] = {0x24, 0x00, 0x00, 0x00, 0xDE, 0xAD, 0xBE, 0xEF};

        obd2_send_frame(REPLY_ID, 8, frame1);
        obd2_send_frame(REPLY_ID, 8, frame2);
        obd2_send_frame(REPLY_ID, 8, frame3);
        obd2_send_frame(REPLY_ID, 8, frame4);
        obd2_send_frame(REPLY_ID, 8, frame5);
    }
    else if (pid == 0xA) // ECU Name
    {
        unsigned char frame1[8] = {0x10, 2 + 20, 0x49, pid, 1, ecu_Name[0], ecu_Name[1], ecu_Name[2]};
        unsigned char frame2[8] = {0x21, ecu_Name[3], ecu_Name[4], ecu_Name[5],
                                   ecu_Name[6], ecu_Name[7], ecu_Name[8], ecu_Name[9]};
        unsigned char frame3[8] = {0x22, ecu_Name[10], ecu_Name[11], ecu_Name[12],
                                   ecu_Name[13], ecu_Name[14], ecu_Name[15], ecu_Name[16]};
        unsigned char frame4[8] = {0x23, ecu_Name[17], ecu_Name[18]};

        obd2_send_frame(REPLY_ID, 8, frame1);
        obd2_send_frame(REPLY_ID, 8, frame2);
        obd2_send_frame(REPLY_ID, 8, frame3);
        obd2_send_frame(REPLY_ID, 8, frame4);
    }
}

// Request data stream service
void obd2_handle_service_2C(byte add_bytes, byte pid, byte *buf)
{
    //const byte service_mode = 0x2C;
    Serial.println("################ obd2_handle_service_2C " + String(add_bytes, 16) + "; " + String(pid, 16) + "; " + String(buf[0], 16) + "; ");

    if (pid == 0xFE)
    {
        obd2_handle_service_01(buf[0], buf[1], buf + 2);
    }
}

void obd2_handle_service_3E_AA_A9(byte service_mode, byte add_bytes, byte pid, byte *buf)
{
    Serial.println("################ obd2_handle_service_3E_AA " + String(service_mode, 16) + "; " + String(add_bytes, 16) + "; " + String(pid, 16) + "; " + String(buf[0], 16) + "; ");

    byte tmp[8] = {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
    obd2_send_frame(REPLY_ID, 8, tmp);
    return;

    /*if ((service_mode == 0x3E || service_mode == 0xA9 || service_mode == 0xAA) && pid == 0x0)
    {
        // byte tmp[4] = {0x00, 0x00, 0x00, 0x00};
        // obd2_create_response_frame(tmp8, service_mode, pid, 4, tmp);

        byte tmp[8] = {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
        obd2_send_frame(REPLY_ID, 8, tmp);
    }

    if (service_mode == 0xAA && pid == 0x3)
    {
        byte tmp[4] = {0x00, 0x00, 0x00, 0x00};
        obd2_create_response_frame(tmp8, service_mode, pid, 4, tmp);
        obd2_send_frame(REPLY_ID, 8, tmp8);
    }

    // Diagnostics test status
    if (service_mode == 0xA9 && pid == 0x81)
    {
        obd2_create_response_frame(tmp8, service_mode, 0);
        obd2_send_frame(REPLY_ID, 8, tmp8);
    }*/
}

void obd2_handle_request_frame(int address, byte add_bytes, byte *buf)
{
    if (buf[0] == 0x1)
    {
        obd2_handle_service_01(add_bytes, buf[1], buf + 1);
    }
    else if (buf[0] == 0x6)
    {
        obd2_handle_service_06(add_bytes, buf[1], buf + 1);
    }
    else if (buf[0] == 0x9)
    {
        obd2_handle_service_09(add_bytes, buf[1], buf + 1);
    }
    else if (buf[0] == 0x2C)
    {
        obd2_handle_service_2C(add_bytes, buf[1], buf + 1);
    }
    else if (buf[0] == 0x3E || buf[0] == 0xAA || buf[0] == 0xA9)
    {
        obd2_handle_service_3E_AA_A9(buf[0], add_bytes, buf[1], buf + 2);
    }
    else if (add_bytes == 1 && (buf[0] == 0x03 || buf[0] == 0x07)) // DTCs
    {
        int service_mode = buf[0];
        // int pid = buf[1];

        Serial.println("################ SERVICE " + String(service_mode));

        if (MIL)
        {
            byte dat[4] = {0};
            int code_count = 0;

            if (service_mode == 0x03)
            {
                dat[0] = 0x04;
                dat[1] = 0x20;
                dat[2] = 0x04;
                dat[3] = 0x30;
                code_count = 2;

                obd2_create_response_frame(tmp8, service_mode, code_count, sizeof(dat) / sizeof(*dat), dat);
                obd2_send_frame(REPLY_ID, 8, tmp8);
            }
            else
            {
                dat[0] = 0x03;
                dat[1] = 0x40;
                code_count = 1;

                obd2_create_response_frame(tmp8, service_mode, code_count, sizeof(dat) / sizeof(*dat), dat);
                obd2_send_frame(REPLY_ID, 8, tmp8);
            }

            // obd2_create_response_frame(tmp8, service_mode, code_count, sizeof(dat) / sizeof(*dat), dat);
            // obd2_send_frame(REPLY_ID, 8, tmp8);
        }
        else
        {
            obd2_create_response_frame(tmp8, service_mode, 0);
            obd2_send_frame(REPLY_ID, 8, tmp8);
        }
    }
    else if (add_bytes == 1 && buf[0] == 0x4) // Clear DTCs
    {
        MIL = false;
    }
}