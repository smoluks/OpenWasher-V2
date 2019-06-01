/*
 * wash_stages.h
 *
 *  Created on: Feb 16, 2019
 *      Author: Shironeko
 */

#pragma once

#include <stdbool.h>
#include <stdint-gcc.h>
#include <valve_driver.h>

bool stage_valve_drawwater(enum valve_e valve, uint8_t level);
bool stage_sink(uint32_t delay);
