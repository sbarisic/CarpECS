#include "api.h"
#include "ecu.h"
#include <stdio.h>

task_state blinker_left_task;
task_state blinker_right_task;

trigger_task blinker_left_trigger;
trigger_task blinker_right_trigger;
trigger_task* triggers[] = { &blinker_left_trigger, &blinker_right_trigger };

volatile int blinker_left_enabled;
volatile int blinker_right_enabled;

void enable_blinker_left(int enable) {
	if (enable)
		printf("LEFT ON\n");
	else
		printf("LEFT OFF\n");
}

void enable_blinker_right(int enable) {
	if (enable)
		printf("RIGHT ON\n");
	else
		printf("RIGHT OFF\n");
}

__declspec(dllexport) void ecu_init() {
	ecu_init_api();

	blinker_left_enabled = 0;
	blinker_right_enabled = 0;

	blinker_left_trigger.action = enable_blinker_left;
	blinker_right_trigger.action = enable_blinker_right;

	trigger_init(&blinker_left_trigger);
	trigger_init(&blinker_right_trigger);

	task_reset(&blinker_left_task, 0);
	task_reset(&blinker_right_task, 0);
}

__declspec(dllexport) void ecu_interrupt(int n) {
	switch (n) {
	case 1:
		blinker_left_enabled = 1;
		break;

	case 2:
		blinker_left_enabled = 0;
		break;

	case 3:
		blinker_right_enabled = 1;
		break;

	case 4:
		blinker_right_enabled = 0;
		break;

	default:
		break;
	}
}

__declspec(dllexport) void ecu_loop() {
	// Tasks
	{
		// Left blinker
		if (blinker_left_enabled != 0) {
			if (task_elapsed(&blinker_left_task)) {
				task_reset(&blinker_left_task, 750);
				trigger_toggle(&blinker_left_trigger);
			}
		} else
			trigger_set(&blinker_left_trigger, 0);

		// Right blinker
		if (blinker_right_enabled != 0) {
			if (task_elapsed(&blinker_right_task)) {
				task_reset(&blinker_right_task, 750);
				trigger_toggle(&blinker_right_trigger);
			}
		} else
			trigger_set(&blinker_right_trigger, 0);
	}

	for (size_t i = 0; i < (sizeof(triggers) / sizeof(*triggers)); i++)
		trigger_activate(triggers[i]);
}