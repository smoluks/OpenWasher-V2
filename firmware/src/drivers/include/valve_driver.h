#pragma once
#include <stdbool.h>

enum valve_e
{
	prewash_valve,
	conditioner_valve,
	both_washmode
};

bool valve_test();

bool valve_drawwater(enum valve_e valve, uint8_t level);
void valve_open(enum valve_e valve);
void valve_close(enum valve_e valve);

