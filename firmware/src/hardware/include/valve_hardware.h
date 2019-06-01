/*
 * valve_hardware.h
 *
 *  Created on: 10 дек. 2018 г.
 *      Author: Администратор
 */

#pragma once

#include <stdbool.h>

enum valve_state
{
	valve_opened,
	valve_retention,
	valve_closed
};

bool is_water();
bool is_overflow();
void valve_emergencyclose();
void valve_crosszero(bool phase);
