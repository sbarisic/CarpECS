#include <ecumaster.h>

void ecumaster_init(emu_data_t *emu_data)
{
}

void ecumaster_handle_frame(uint32_t can_id, byte can_dlc, byte *data, emu_data_t *emu_data)
{
    // This decodes the frames and fills them into the data:
    if (emu_data == NULL)
        return;

    // Base:
    if (can_id == emu_data->CAN_Base)
    {
        // 0-1 RPM in 16Bit unsigned
        emu_data->RPM = (data[1] << 8) + data[0];
        // 2 TPS in /2 %
        emu_data->TPS = data[2] * 0.5;
        // 3 IAT 8bit signed -40-127°C
        emu_data->IAT = int8_t(data[3]);
        // 4-5 MAP 16Bit 0-600kpa
        emu_data->MAP = (data[5] << 8) + data[4];
        // 6-7 INJPW 0-50 0.016129ms
        emu_data->pulseWidth = ((data[7] << 8) + data[6]) * 0.016129;
    }
    // Base +1:
    if (can_id == emu_data->CAN_Base + 1)
    {
        // AIN in 16Bit unsigned  0.0048828125 V/bit
        emu_data->analogIn1 = ((data[1] << 8) + data[0]) * 0.0048828125;
        emu_data->analogIn2 = ((data[3] << 8) + data[2]) * 0.0048828125;
        emu_data->analogIn3 = ((data[5] << 8) + data[4]) * 0.0048828125;
        emu_data->analogIn4 = ((data[7] << 8) + data[6]) * 0.0048828125;
    }
    // Base +2:
    if (can_id == emu_data->CAN_Base + 2)
    {
        // 0-1 VSPD in 16Bit unsigned 1 kmh/h / bit
        emu_data->vssSpeed = (data[1] << 8) + data[0];
        // 2 BARO 50-130 kPa
        emu_data->Baro = data[2];
        // 3 OILT 0-160°C
        emu_data->oilTemperature = data[3];
        // 4 OILP BAR 0.0625 bar/bit
        emu_data->oilPressure = data[4] * 0.0625;
        // 5 FUELP BAR 0.0625 bar/bit
        emu_data->fuelPressure = data[5] * 0.0625;
        // 6-7 CLT 16bit Signed -40-250 1 C/bit
        emu_data->CLT = int16_t(((data[7] << 8) + data[6]));
    }
    // Base +3:
    if (can_id == emu_data->CAN_Base + 3)
    {
        // 0 IGNANG in 8Bit signed    -60 60  0.5deg/bit
        emu_data->IgnAngle = int8_t(data[0]) * 0.5;
        // 1 DWELL 0-10ms 0.05ms/bit
        emu_data->dwellTime = data[1] * 0.05;
        // 2 LAMBDA 8bit 0-2 0.0078125 L/bit
        emu_data->wboLambda = data[2] * 0.0078125;
        // 3 LAMBDACORR 75-125 0.5%
        emu_data->LambdaCorrection = data[3] * 0.5;
        // 4-5 EGT1 16bit °C
        emu_data->Egt1 = ((data[5] << 8) + data[4]);
        // 6-7 EGT2 16bit °C
        emu_data->Egt2 = ((data[7] << 8) + data[6]);
    }
    // Base +4:
    if (can_id == emu_data->CAN_Base + 4)
    {
        // 0 GEAR
        emu_data->gear = data[0];
        // 1 ECUTEMP °C
        emu_data->emuTemp = data[1];
        // 2-3 BATT 16bit  0.027 V/bit
        emu_data->Batt = ((data[3] << 8) + data[2]) * 0.027;
        // 4-5 ERRFLAG 16bit
        emu_data->cel = ((data[5] << 8) + data[4]);
        // 6 FLAGS1 8bit
        emu_data->flags1 = data[6];
        // 7 ETHANOL %
        emu_data->flexFuelEthanolContent = data[7];
    }
    // Base +5:
    if (can_id == emu_data->CAN_Base + 5)
    {
        // 0 DBW Pos 0.5%
        emu_data->DBWpos = data[0] * 0.5;
        // 1 DBW Target 0.5%
        emu_data->DBWtarget = data[1] * 0.5;
        // 2-3 TC DRPM RAW 16bit  1/bit
        emu_data->TCdrpmRaw = ((data[3] << 8) + data[2]);
        // 4-5 TC DRPM 16bit  1/bit
        emu_data->TCdrpm = ((data[5] << 8) + data[4]);
        // 6 TC Torque reduction %
        emu_data->TCtorqueReduction = data[6];
        // 7 Pit Limit Torque reduction %
        emu_data->PitLimitTorqueReduction = data[7];
    }
    // Base +6:
    if (can_id == emu_data->CAN_Base + 6)
    {
        // AIN in 16Bit unsigned  0.0048828125 V/bit
        emu_data->analogIn5 = ((data[1] << 8) + data[0]) * 0.0048828125;
        emu_data->analogIn6 = ((data[3] << 8) + data[2]) * 0.0048828125;
        emu_data->outflags1 = data[4];
        emu_data->outflags2 = data[5];
        emu_data->outflags3 = data[6];
        emu_data->outflags4 = data[7];
    }
    // Base +7:
    if (can_id == emu_data->CAN_Base + 7)
    {
        // 0-1 Boost target 16bit 0-600 kPa
        emu_data->boostTarget = ((data[1] << 8) + data[0]);
        // 2 PWM#1 DC 1%/bit
        emu_data->pwm1 = data[2];
        // 3 DSG mode 2=P 3=R 4=N 5=D 6=S 7=M 15=fault
        emu_data->DSGmode = data[3];
        // since version 143 this contains more data, check length:
        if (can_dlc == 8)
        {
            // 4 Lambda target 8bit 0.01%/bit
            emu_data->lambdaTarget = data[4] * 0.01;
            // 5 PWM#2 DC 1%/bit
            emu_data->pwm2 = data[5];
            // 6-7 Fuel used 16bit 0.01L/bit
            emu_data->fuel_used = ((data[7] << 8) + data[6]) * 0.01;
        }
    }
}