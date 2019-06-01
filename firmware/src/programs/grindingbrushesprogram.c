/*
 * притирка щеток
 */

#include <delay.h>
#include <engine_driver.h>
#include <stdbool.h>
#include <stdint-gcc.h>

bool grindingbrushesint_go();

extern volatile bool ct;

bool grindingbrushes_go(uint8_t number, uint8_t* args)
{
	bool result = grindingbrushesint_go();

	engine_settargetrps(0, off);

	return result;
}

bool grindingbrushesint_go()
{
	engine_settargetrps(1, cw);
	delay_ms_with_ct(1800000u);
	if (ct)
		return false;

	engine_settargetrps(0, off);
	delay_ms_with_ct(5000u);
	if (ct)
		return false;

	engine_settargetrps(1, ccw);
	delay_ms_with_ct(1800000u);
	if (ct)
		return false;

	engine_settargetrps(0, off);
	delay_ms_with_ct(5000u);
	if (ct)
		return false;

	engine_settargetrps(20, cw);
	delay_ms_with_ct(300000u);
	if (ct)
		return false;

	engine_settargetrps(0, off);
	delay_ms_with_ct(5000u);
	if (ct)
		return false;

	engine_settargetrps(20, ccw);
	delay_ms_with_ct(300000u);
	if (ct)
		return false;

	engine_settargetrps(0, off);
	delay_ms_with_ct(5000u);
	if (ct)
		return false;

	return true;
}
