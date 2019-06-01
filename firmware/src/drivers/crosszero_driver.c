#include <stdbool.h>
#include "crosszero_driver.h"
#include "engine_driver.h"
#include "therm_driver.h"
#include "valve_hardware.h"
#include "systick.h"

void crosszero_failed();

uint32_t crosszerotimestamp = 0;
bool crosszeropresent = false;

inline void crosszero_irq(bool phase) {
	uint32_t crosszerotime = delta(crosszerotimestamp);

	if (crosszerotime > 8 && crosszerotime < 12) {
		crosszeropresent = true;
		engine_crosszero();
		therm_crosszero();
		valve_crosszero(phase);
	} else
		crosszero_failed();

	crosszerotimestamp = get_systime();
}

inline void crosszero_systick() {
	if (!checkdelay(crosszerotimestamp, 11))
		crosszero_failed();
}

inline bool is_crosszero_present() {
	return crosszeropresent;
}

void crosszero_failed() {
	crosszeropresent = false;
}
