#pragma once

#include <stdbool.h>

void crosszero_irq(bool phase);
void crosszero_systick();
bool is_crosszero_present();
