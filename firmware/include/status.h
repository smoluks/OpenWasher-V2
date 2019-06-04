#include "stm32f10x.h"

#pragma pack(push, 1)

enum stage_e
{
	STATUS_STOP = 0,
	STATUS_CLOSE_DOOR = 1,
	STATUS_OPEN_DOOR = 2,
	STATUS_RINSING = 3,
	STATUS_SPINNING = 4,
	STATUS_PREWASHING = 5,
	STATUS_WASHING = 6,
	STATUS_DRAWWATER = 7,
	STATUS_SINK = 8,
	STATUS_SELFTESTING = 9,
};

struct status_s
{
	uint8_t program;
	uint8_t stage;
	uint8_t temperature;
	uint32_t timefull;
	uint32_t timepassed;
};

#pragma pack(pop)

uint8_t* get_status();
void status_set_program(uint8_t program);
void status_set_stage(enum stage_e stage);
