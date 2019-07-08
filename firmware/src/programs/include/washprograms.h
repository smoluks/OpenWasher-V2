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
	{70, 137, 1, 10},
	{70, 120, 1, 10},
	{60, 105, 1, 10},
	{40, 72, 1, 10},
	{30, 60, 1, 10},
	{60, 77, 1, 3},
	{50, 73, 1, 3},
	{40, 58, 1, 3},
	{30, 30, 1, 3},
	{40, 50, 1, 0},
	{30, 45, 1, 0}
};
