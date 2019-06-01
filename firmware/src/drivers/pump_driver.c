#include <delay.h>
#include <error.h>
#include <pump_driver.h>
#include <pump_hardware.h>
#include <stdint-gcc.h>
#include <stdio.h>
#include <systick.h>
#include <valve_driver.h>
#include <valve_hardware.h>

extern volatile bool ct;

volatile bool pumpfeedbackispresent = false;

bool pump_test()
{
	printf("Test pump\n");

	pump_relayenable();

	uint32_t timestamp;
	getsystime(&timestamp);
	while (!pumpfeedbackispresent && checkdelay(timestamp, 1000));
	if (!pumpfeedbackispresent)
	{
		set_error(BAD_PUMP);
		return false;
	}
	printf("Pump relay on at %u ms\n", delta(timestamp));

	pump_triakenable();
	getsystime(&timestamp);
	while (pumpfeedbackispresent && checkdelay(timestamp, 1000));
	if (pumpfeedbackispresent)
	{
		set_error(BAD_PUMP_TRIAK);
		return false;
	}
	printf("Pump on at %u ms\n", delta(timestamp));

	while(is_water() && !ct);
	delay_ms_with_ct(15000u);
	pump_triakdisable();
	if(ct)
	{
		pump_relaydisable();
		return false;
	}

	getsystime(&timestamp);
	while (!pumpfeedbackispresent && checkdelay(timestamp, 1000));
	if (!pumpfeedbackispresent)
	{
		set_error(BAD_PUMP_TRIAK2);
		return false;
	}
	printf("Pump off at %u ms\n", delta(timestamp));

	pump_relaydisable();
	getsystime(&timestamp);
	while (pumpfeedbackispresent && checkdelay(timestamp, 1000));
	if (pumpfeedbackispresent)
	{
		set_error(BAD_PUMP_RELAY);
		return false;
	}
	printf("Pump relay off at %u ms\nTest pump OK\n", delta(timestamp));

	return true;
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
	pump_relayenable();

	uint32_t timestamp;
	getsystime(&timestamp);
	while (!pumpfeedbackispresent && checkdelay(timestamp, 5000));
	if (!pumpfeedbackispresent)
	{
		set_error(BAD_PUMP);
		return false;
	}

	pump_triakenable();
	getsystime(&timestamp);
	while (pumpfeedbackispresent && checkdelay(timestamp, 2000));
	if (pumpfeedbackispresent)
	{
		set_error(BAD_PUMP_TRIAK);
		return false;
	}
	return true;
}

bool pump_disable()
{
	pump_triakdisable();

	uint32_t timestamp;
	getsystime(&timestamp);
	while (!pumpfeedbackispresent && checkdelay(timestamp, 1000));
	if (!pumpfeedbackispresent)
	{
		set_error(BAD_PUMP_TRIAK2);
		return false;
	}

	pump_relaydisable();

	getsystime(&timestamp);
	while (pumpfeedbackispresent && checkdelay(timestamp, 1000));
	if (pumpfeedbackispresent)
	{
		set_error(BAD_PUMP_RELAY);
		return false;
	}

	return true;
}

uint32_t pumpprocesstime = 0;
inline void pump_systick()
{
	if (pumpfeedbackispresent && !checkdelay(pumpprocesstime, 15))
		pumpfeedbackispresent = false;
}

inline void pump_feedback()
{
	uint32_t diff = delta(pumpprocesstime);
	if(diff >= 8 && diff <= 12)
		pumpfeedbackispresent = true;

	 getsystime(&pumpprocesstime);
}

