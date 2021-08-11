#include "api.h"

void ecu_init_api() {
	ftime(&start);
}

uint16_t millis() {
	struct timeb end;
	ftime(&end);

	return (int)(1000.0f * (end.time - start.time) + (end.millitm - start.millitm));
}