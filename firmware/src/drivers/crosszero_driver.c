#include <stdbool.h>
#include "crosszero_driver.h"
#include "engine_driver.h"
#include "therm_driver.h"
#include "valve_hardware.h"
#include "systick.h"

void crosszero_failed();

uint32_t crosszerotimestamp = 0;
uint32_t crosszerotime = 0;
volatile bool crosszeropresent = false;

inline void crosszero_irq(bool phase) {
	crosszerotime += delta(crosszerotimestamp);

	if (crosszerotime > 8 && crosszerotime < 12) {
		engine_crosszero();
		therm_crosszero();
		valve_crosszero(phase);

		crosszeropresent = true;
		crosszerotime = 0;
	} else if(crosszerotime >= 12){
		crosszero_failed();
		crosszerotime = 0;
	}

	crosszerotimestamp = get_systime();
}

inline void crosszero_systick() {
	if (!checkdelay(crosszerotimestamp, 12))
		crosszero_failed();
}

inline bool is_crosszero_present() {
	return crosszeropresent;
}

void crosszero_failed() {
	crosszeropresent = false;
	//error handlers
}
