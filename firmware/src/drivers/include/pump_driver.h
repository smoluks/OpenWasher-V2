#pragma once
#include "stm32f10x.h"
#include <stdbool.h>

bool pump_enable();
bool pump_disable();

void pump_systick();
void pump_feedback();
bool pump_test();
bool sink(uint32_t ms);
