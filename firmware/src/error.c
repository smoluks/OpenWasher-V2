#include <engine_driver.h>
#include "error.h"
#include <stdbool.h>
#include <therm_driver.h>
#include <therm_driver.h>
#include <uart_driver.h>
#include <valve_driver.h>

extern volatile bool ct;
extern enum errorcode error;

void set_error(enum errorcode code)
{
	therm_emergencydisable();
	valve_emergencyclose();
	engine_emergencystop();
	//
	error = code;
	ct = true;
}

void set_warning(enum errorcode code)
{
	send_error(code);
}

void HardFault_Handler()
{
	set_error(HardFault);
}

void MemManage_Handler()
{
	set_error(MemManageFault);
}

void BusFault_Handler()
{
	set_error(BusFault);
}

void UsageFault_Handler()
{
	set_error(UsageFault);
}
