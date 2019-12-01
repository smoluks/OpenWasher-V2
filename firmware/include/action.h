#pragma once

enum {
	ACTION_STARTPROGRAM = 1,
	ACTION_STOPPROGRAM = 2,
	ACTION_GOTOBOOTLOADER = 4,
};

void processAction();
