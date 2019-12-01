#include <systick.h>
#include <stdbool.h>
#include "delay.h"
#include "watchdog.h"

extern volatile bool ct;

inline void delay_us(uint16_t us)
{
	volatile uint32_t i = us * uS;
	while ((i--) != 0);
}

inline void delay_ms(uint32_t ms)
{
	uint32_t timestamp;
	getsystime(&timestamp);
	while (checkdelay(timestamp, ms))
	{
		WDT_RESET;
	}
}

inline void delay_ms_with_ct(uint32_t ms)
{
	uint32_t timestamp;
	getsystime(&timestamp);
	while (checkdelay(timestamp, ms) && !ct)
	{
		WDT_RESET;
	}
}

