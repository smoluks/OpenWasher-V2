#pragma once

#include <stdbool.h>

void engine_setdirectioncw();
void engine_setdirectionccw();
void engine_setdirectionoff();

bool engine_isdirectioncw();
bool engine_isdirectionccw();
bool engine_isanydirection();

void engine_triakon();
void engine_triakoff();
bool get_engine_feedback_phase();
