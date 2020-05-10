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

uint32_t stage_wash_get_duration(uint8_t duration);
bool stage_wash(uint8_t temp, uint8_t duration, uint8_t rps, enum valve_e valve, uint8_t waterlevel);
