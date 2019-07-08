#pragma once

#include "system_stm32f10x.h"
#include "emptyprogram.h"
#include "grindingbrushesprogram.h"
#include "rinsingprogram.h"
#include "sinkingprogram.h"
#include "spinningprogram.h"
#include "testprogram.h"
#include "washingprogram.h"
#include "waterheaterprogram.h"
#include "pidprogram.h"
#include <stdbool.h>

#define PROGRAM_COUNT 21

typedef bool (*fn_program)(options args);

const fn_program programs[PROGRAM_COUNT] =
{
	testprogram_go, 			//0
	wash_go,					//1
	wash_go,					//2
	wash_go,					//3
	wash_go,					//4
	wash_go,					//5
	wash_go,					//6
	wash_go,					//7
	wash_go,					//8
	wash_go,					//9
	wash_go,					//10
	wash_go,					//11
	rinsingprogram_go,			//12
	rinsingprogram_go,			//13
	spinningprogram_go,			//14
	delicatespinningprogram_go,	//15
	sink_go,					//16
	sink_open_valve_go,			//17
	waterheater_go,				//18
	grindingbrushes_go,			//19
	pid_go,						//20
};
