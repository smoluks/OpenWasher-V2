#pragma once

#include <stdbool.h>
#include "programs.h"
#include "programOptions.h"

#include "grindingbrushesprogram.h"
#include "pidprogram.h"
#include "rinsingprogram.h"
#include "sinkingprogram.h"
#include "spinningprogram.h"
#include "testprogram.h"
#include "washProgram.h"
#include "waterheaterprogram.h"

typedef bool (*fn_program)(program programNumber, programOptions programOptions);

const fn_program programs[PROGRAM_COUNT] =
{
	processTestProgram, 				//0
	processWashProgram,					//1
	processWashProgram,					//2
	processWashProgram,					//3
	processWashProgram,					//4
	processWashProgram,					//5
	processWashProgram,					//6
	processWashProgram,					//7
	processWashProgram,					//8
	processWashProgram,					//9
	processWashProgram,					//10
	processWashProgram,					//11
	processRinsingProgram,				//12
	processRinsingProgram,				//13
	processSpinningProgram,				//14
	processSpinningProgram,			    //15
	processSinkingProgram,				//16
	processSinkingProgramWithOpenValve,	//17
	processWaterHeaterCommand,			//18
	processGrindingBrushes,				//19
	processPidProgram,					//20
};
