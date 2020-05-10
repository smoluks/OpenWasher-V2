#pragma once

#include <stdbool.h>
#include <stdint-gcc.h>

uint32_t stage_rinsing_get_duration(uint8_t count);
bool stage_rinsing(uint8_t count, uint8_t speed, uint8_t waterlevel);
