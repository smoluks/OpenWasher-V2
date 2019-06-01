#pragma once
#include <stdbool.h>

bool door_testlock();
bool door_testunlock();

bool door_lock();
bool door_unlock();
void door_systick();
void door_crosszero();
