#include "ecu.h"

#ifdef WINDOWS_TEST
#include "api.h"
#endif

void task_reset(task_state* ts, uint16_t ms) {
	ts->trigger_time = millis() + ms;
	ts->enabled = 1;
}

void task_delay(task_state* ts, uint16_t ms) {
	ts->trigger_time += ms;
}

int task_elapsed(task_state* ts) {
	if (!ts->enabled)
		return 0;

	return ts->trigger_time <= millis();
}

void task_enable(task_state* ts, int enabled) {
	ts->enabled = enabled;
}

// Triggers

void trigger_activate(trigger_task* t) {
	if (t->dirty) {
		t->dirty = 0;
		t->action(t->enable);
	}
}

void trigger_set(trigger_task* t, int enable) {
	if (t->enable != enable) {
		t->enable = enable;
		t->dirty = 1;
	}
}

void trigger_toggle(trigger_task* t) {
	trigger_set(t, !t->enable);
}

void trigger_init(trigger_task* t) {
	t->enable = 0;
	t->dirty = 1;
}