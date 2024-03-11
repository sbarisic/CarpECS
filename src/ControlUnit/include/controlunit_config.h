#include <Arduino.h>

#ifdef CONTROLUNIT_IMPL
#define DEFINE_GLOBAL(type, name, val) type name = val
#else
#define DEFINE_GLOBAL(type, name, val) extern type name
#endif

DEFINE_GLOBAL(byte, vehicle_Vin[18], "W0V0XEP68K4028819");    // Vechicle VIN
DEFINE_GLOBAL(byte, calibration_ID[18], "86ACHMK48173ZYT0"); // Calibration ID
DEFINE_GLOBAL(byte, cvn_ID[18], "12345678910111213");         // CVN
DEFINE_GLOBAL(byte, ecu_Name[19], "ECM-EngineControl");       // ECU Name
DEFINE_GLOBAL(byte, obd_Std, 6);          // OBD standards https://en.wikipedia.org/wiki/OBD-II_PIDs#Service_01_PID_1C
DEFINE_GLOBAL(byte, fuel_Type, 1);        // Fuel Type Coding https://en.wikipedia.org/wiki/OBD-II_PIDs#Fuel_Type_Coding
DEFINE_GLOBAL(char, FW_Version[], "1.0"); // Current Firmware Version

DEFINE_GLOBAL(bool, MIL, false);
DEFINE_GLOBAL(byte, CEL_Codes, 0);