#pragma once

#include <stdbool.h>

enum direction_e
{
	off = 0,
	cw = 1,
	ccw = 2,
};

void engine_setdirectioncw();
void engine_setdirectionccw();
void engine_setdirectionoff();

enum direction_e get_direction();

void engine_triakon();
void engine_triakoff();

