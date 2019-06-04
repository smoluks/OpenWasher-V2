#pragma once

#include <stdint.h>

#pragma pack(push, 1)

typedef struct
{
	uint8_t number;
	uint8_t temperature;
	uint8_t delay;
	uint8_t washingspeed;
	uint8_t spinningspeed;
	uint8_t waterlevel;
	uint8_t rinsingcycles;
} options;

#pragma pack(pop)
