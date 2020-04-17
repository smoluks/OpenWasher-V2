import 'dart:async';
import 'dart:typed_data';

import 'PacketManager.dart';

class ProtocolManager {
  Timer _pollingTimer;
  PacketManager _packetManager = new PacketManager();

  //var stateStream = new StreamController<WasherState>();

  ProtocolManager(String btAddress) {
    _packetManager.connect(btAddress, onReceived, onClose);
    //_pollingTimer = new Timer.periodic(
    //    Duration(seconds: 10), (_) async => await updateState());
  }

  void dispose() {
    //_pollingTimer?.cancel();
    //stateStream.close();
    _packetManager.disconnect();
  }

  void onClose() {}

  void onReceived(Uint8List data) {}

  WasherState getState() {
    //_packetManager.
  }
}

class WasherState {
  bool isRunning;
  Program program;
  Stage stage;
  int temperature;
  int speed;
  int programFullTime;
  int programPastTime;
}

enum Program { SelfTesting, Program1 }

enum Stage {
  Stop,
  LockDoor,
  UnlockDoor,
  Rinsing,
  Spinning,
  Prewashing,
  Washing,
  DrawWater,
  Sink,
  SelfTesting,
}
