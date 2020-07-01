#include "stm32f10x.h"

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
	STATUS_GRINDINGBRUSHES = 10,
};

#pragma pack(push, 1)

typedef struct
{
	uint8_t program;
	uint8_t stage;
	uint8_t temperature;
	uint8_t rotationSpeed;
	uint8_t error;
	uint32_t programDuration;
	uint32_t programTimePassed;
} Status;

#pragma pack(pop)

uint8_t* buildCurrentStatus();
void status_set_program(uint8_t program, uint32_t fullTimeLength);
void status_set_stage(enum stage_e stage);
