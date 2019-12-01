#include <engine_driver.h>
#include <pump_driver.h>
#include <door_driver.h>
#include <stdint-gcc.h>
#include <therm_driver.h>
#include "crosszero_driver.h"
#include "led_driver.h"
#include "systick.h"

volatile uint32_t systime = 0;
extern volatile bool ct;

//SysTick Interrupt
void SysTick_Handler(void)
{
	systime++;

	crosszero_systick();
	pump_systick();
	engine_systick();
	therm_systick();
	led_systick();
}

inline uint32_t get_systime()
{
	return systime;
}

void getsystime(uint32_t* timestamp)
{
	*timestamp = systime;
}

uint32_t delta(uint32_t timestamp)
{
	return systime - timestamp;
}

bool checkdelay(uint32_t timestamp, uint32_t delay)
{
	return delta(timestamp) < delay;
}

bool check_time_passed(uint32_t timestamp)
{
	return (timestamp - systime) > 0x80000000;
}

bool check_time_passed_with_ct(uint32_t timestamp)
{
	return ct || ((timestamp - systime) & 0x80000000);
}
