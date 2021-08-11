#pragma once
#include <stdint.h>

typedef struct {
	int enabled;
	uint16_t trigger_time;
} task_state;

void task_reset(task_state* ts, uint16_t ms);
void task_delay(task_state* ts, uint16_t ms);
int task_elapsed(task_state* ts);
void task_enable(task_state* ts, int enabled);

typedef void(*trigger_action)(int enable);

typedef struct {
	int enable;
	trigger_action action;

	volatile int dirty;
} trigger_task;

void trigger_activate(trigger_task* t);
void trigger_set(trigger_task* t, int enable);
void trigger_toggle(trigger_task* t);
void trigger_init(trigger_task* t);
