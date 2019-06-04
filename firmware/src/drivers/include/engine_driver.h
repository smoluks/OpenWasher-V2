#pragma once

#include <stdbool.h>
#include "engine_hardware.h"
#include "stm32f10x.h"

#define VALUE_FULL 32768

enum engine_feedback_state_e
{
	ef_off = 0,
	ef_half = 1,
	ef_full = 2,
	ef_regul = 3
};


bool engine_test();
bool engine_settargetrps(uint8_t rps, enum direction_e direction);
void engine_emergencystop();

void process_feedback(enum engine_feedback_state_e current);
void engine_crosszero();
void engine_systick();
void process_tacho(uint16_t value);


enum engine_feedback_state_e get_engine_feedback();


