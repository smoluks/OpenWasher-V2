#pragma once
#include <stdbool.h>
#include <stdint.h>

#define THERM_HYSTERESIS 1

bool therm_test();
bool set_temperature(uint8_t t);
uint8_t get_temperature();
void therm_crosszero();
void therm_emergencydisable();

void therm_systick();
void therm_adc(uint16_t value);
void therm_feedback();


