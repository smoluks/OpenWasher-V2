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

bool stage_wash(uint8_t temp, uint8_t duration, uint8_t rps, enum valve_e valve, uint8_t waterlevel);
