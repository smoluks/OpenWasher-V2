#pragma once
#include "stm32f10x.h"
#include <stdbool.h>

bool pump_test();
bool sink(uint32_t ms);
bool sink_if_water(uint32_t ms);
bool pump_enable();
bool pump_disable();

void pump_systick();
void pump_feedback();


