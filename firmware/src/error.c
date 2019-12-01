#include <stdbool.h>
#include "therm_driver.h"
#include "therm_driver.h"
#include "uart_driver.h"
#include "valve_driver.h"
#include "engine_driver.h"
#include "valve_hardware.h"
#include "error.h"

extern volatile bool ct;

enum errorcode error = NOERROR;

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

void NMI_Handler()
{
	while(true);
}

