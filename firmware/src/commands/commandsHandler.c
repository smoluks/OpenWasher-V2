#include <stdint.h>
#include "commandsRoutine.h"

#include "configCommand.h"
#include "defaultOptionsCommand.h"
#include "goToBootloaderModeCommand.h"
#include "startProgramCommand.h"
#include "statusCommand.h"
#include "stopProgramCommand.h"
#include "timeCommand.h"

void processCommand(uint8_t* buffer, uint8_t count) {
	if(!count)
		send_empty_answer();
	else switch (buffer[0]) {
	case startProgramPacketType:
		processStartProgramCommand(buffer+1, count-1);
		break;
	case stopProgramPacketType:
		processStopProgramCommand(buffer+1, count-1);
		break;
	case getProgramDefaultsPacketType:
		processDefaultOptionsCommand(buffer+1, count-1);
		break;
	case goToBootloaderModePacketType:
		processGoToBootloaderModeCommand(buffer+1, count-1);
		break;
	case getStatusPacketType:
		processStatusCommand(buffer+1, count-1);
		break;
	case setSetConfigPacketType:
		processConfigCommand(buffer+1, count-1);
		break;
	case getSetTimePacketType:
		processTimeCommand(buffer+1, count-1);
		break;
	default:
		send_answer(buffer[0], UNSUPPORTEDCOMMAND);
		break;
	}
}


