#pragma once

#include "system_stm32f10x.h"
#include <stdbool.h>

bool sink_go(uint8_t number, uint8_t* args);
bool sink_open_valve_go(uint8_t number, uint8_t* args);
