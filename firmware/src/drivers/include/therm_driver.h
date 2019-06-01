#pragma once
#include <stdbool.h>

bool therm_test();

bool set_temperature(uint8_t t);
uint8_t get_temperature();

void therm_emergencydisable();

void therm_systick();

void processRawTemperature(uint16_t temp);

void therm_feedback();
void therm_crosszero();

