#pragma once

#include <stdbool.h>
#include "stm32f10x.h"

enum enginedlfeedbackstate
{
	ef_off = 0,
	ef_half = 1,
	ef_full = 2,
	ef_regul = 3
};

enum direction_e
{
	off = 0,
	cw = 1,
	ccw = 2,
};

void process_feedback(enum enginedlfeedbackstate current);

bool engine_settargetrps(uint8_t rps, uint8_t direction);
bool engine_stop();
void engine_emergencystop();

void engine_systick();
void engine_crosszero();
void process_tacho(uint16_t value);
bool engine_test();

#define P 256
#define I 36
#define D 2048
#define IMAX 32737
#define IMIN 0
#define VALUE_FULL 32737
