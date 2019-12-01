#pragma once

#define PROGRAM_COUNT 21

typedef enum {
	TestProgram,
	Wash1Program,
	Wash2Program,
	Wash3Program,
	Wash4Program,
	Wash5Program,
	Wash6Program,
	Wash7Program,
	Wash8Program,
	Wash9Program,
	Wash10Program,
	Wash11Program,
	RinsingProgram,
	DelicateRinsingProgram,
	SpinningProgram,
	DelicateSpinningProgram,
	SinkProgram,
	SinkPipesProgram,
	WaterHeaterProgram,
	GrindingBrushesProgram,
	CalibratePIDProgram,
	NoProgram = 0xFF
} program;

