#include <stdio.h>
#include <stdbool.h>
#include "pump_driver.h"
#include "pump_hardware.h"
#include "valve_hardware.h"
#include "systick.h"
#include "delay.h"

extern volatile bool ct;

volatile bool pumpfeedbackispresent = false;

bool pump_test()
{
	printf("Test pump\n");

	//check relay on
	pump_relayenable();

	uint32_t timestamp = get_systime();
	while (!pumpfeedbackispresent && checkdelay(timestamp, 1000));
	if (!pumpfeedbackispresent)
	{
		set_error(BAD_PUMP);
		return false;
	}
	printf("Pump relay on at %lu ms\n", delta(timestamp));

	//check triak on
	timestamp = get_systime();
	pump_triakenable();
	while (pumpfeedbackispresent && checkdelay(timestamp, 1000));
	if (pumpfeedbackispresent)
	{
		set_error(BAD_PUMP_TRIAK);
		return false;
	}
	printf("Pump on at %lu ms\n", delta(timestamp));

	//sinking
	while(is_water() && !ct);
	delay_ms_with_ct(15000u);
	if(ct)
	{
		pump_relaydisable();
		return false;
	}

	//check triak off
	timestamp = get_systime();
	pump_triakdisable();
	while (!pumpfeedbackispresent && checkdelay(timestamp, 1000));
	if (!pumpfeedbackispresent)
	{
		set_error(BAD_PUMP_TRIAK2);
		return false;
	}
	printf("Pump off at %lu ms\n", delta(timestamp));

	//check relay off
	timestamp = get_systime();
	pump_relaydisable();
	while (pumpfeedbackispresent && checkdelay(timestamp, 1000));
	if (pumpfeedbackispresent)
	{
		set_error(BAD_PUMP_RELAY);
		return false;
	}
	printf("Pump relay off at %lu ms\nTest pump OK\n", delta(timestamp));

	return true;
}

bool sink_if_water(uint32_t ms)
{
	if(!is_water())
		return true;

	if(!pump_enable())
		return false;

	while(is_water() && !ct);
	delay_ms_with_ct(ms);
	if(ct)
	{
		pump_disable();
		return false;
	}

	return pump_disable();
}

bool sink(uint32_t ms)
{
	if(!pump_enable())
		return false;

	while(is_water() && !ct);
	delay_ms_with_ct(ms);
	if(ct)
	{
		pump_disable();
		return false;
	}

	return pump_disable();
}

bool pump_enable()
{
	uint32_t timestamp = get_systime();
	pump_relayenable();
	while (!pumpfeedbackispresent && checkdelay(timestamp, 1000));
	if (!pumpfeedbackispresent)
	{
		set_error(BAD_PUMP);
		return false;
	}

	pump_triakenable();
	timestamp = get_systime();
	while (pumpfeedbackispresent && checkdelay(timestamp, 1000));
	if (pumpfeedbackispresent)
	{
		set_error(BAD_PUMP_TRIAK);
		return false;
	}

	return true;
}

bool pump_disable()
{
	uint32_t timestamp = get_systime();
	pump_triakdisable();
	while (!pumpfeedbackispresent && checkdelay(timestamp, 1000));
	if (!pumpfeedbackispresent)
	{
		set_error(BAD_PUMP_TRIAK2);
		return false;
	}

	timestamp = get_systime();
	pump_relaydisable();
	while (pumpfeedbackispresent && checkdelay(timestamp, 1000));
	if (pumpfeedbackispresent)
	{
		set_error(BAD_PUMP_RELAY);
		return false;
	}

	return true;
}

uint32_t pumpprocesstime = 0;
inline void pump_feedback()
{
	uint32_t diff = delta(pumpprocesstime);
	if(diff >= 8 && diff <= 12)
		pumpfeedbackispresent = true;

	pumpprocesstime = get_systime();
}

inline void pump_systick()
{
	if (pumpfeedbackispresent && !checkdelay(pumpprocesstime, 13))
		pumpfeedbackispresent = false;
}


