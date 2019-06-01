#pragma once

#include "stm32f10x.h"
#include <stdbool.h>

#define uS 9

void getsystime(uint32_t* timestamp);
uint32_t get_systime();
uint32_t delta(uint32_t timestamp);
bool checkdelay(uint32_t timestamp, uint32_t delay);
