/*
 * washprograms.h
 *
 *  Created on: 3 θών. 2019 γ.
 *      Author: Shironeko
 */

#pragma once

#include <stdint.h>

typedef struct {
	uint8_t temperature;
	uint8_t delay;
	uint8_t washingspeed;
	uint8_t spinningspeed;
} washprogram;

const washprogram washprograms[11] =
{
	{70, 137, 3, 10},
	{70, 120, 3, 10},
	{60, 105, 3, 10},
	{40, 72, 3, 10},
	{30, 60, 3, 10},
	{60, 77, 3, 3},
	{50, 73, 3, 3},
	{40, 58, 3, 3},
	{30, 30, 3, 3},
	{40, 50, 1, 0},
	{30, 45, 1, 0}
};
