/*
 * притирка щеток
 */

#include <stdbool.h>
#include <stdint.h>
#include "options.h"
#include "engine_driver.h"
#include "delay.h"
#include "grindingbrushesprogram.h"

bool grindingbrushesint_go();

extern volatile bool ct;

bool grindingbrushes_go(__attribute__((unused)) options args)
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
