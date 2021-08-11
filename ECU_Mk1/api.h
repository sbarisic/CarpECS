#pragma once

#include <stdint.h>
#include <sys\timeb.h>
#include <Windows.h>

struct timeb start;

void ecu_init_api();
uint16_t millis();