import 'dart:async';
import 'dart:typed_data';

import 'package:openwasher/Dto/PacketType.dart';
import 'package:openwasher/Dto/Program.dart';
import 'package:openwasher/Dto/Stage.dart';
import 'package:openwasher/Dto/WasherState.dart';

import 'PacketManager.dart';

class ProtocolManager {
  static final ProtocolManager _instance = ProtocolManager._protocolManager();

  static ProtocolManager get instance => _instance;

  //
  Timer _pollingTimer;
  PacketManager _packetManager = new PacketManager();

  Completer<WasherState> _stateCompleter;

  Completer<void> _startProgramCompleter;

  Completer<void> _pingCompleter;
  Completer<void> _commandStartCompleter;
  Completer<void> _commandStopCompleter;
  Completer<Params> _getDefaultParamsCompleter;

  StreamController<WasherState> stateStreamController;
  Stream<WasherState> stateStream;

  ProtocolManager._protocolManager() {
    stateStreamController = new StreamController<WasherState>();
    stateStream = stateStreamController.stream.asBroadcastStream();

    print('ProtocolManager created');
  }

  Stream<WasherState> onStateChange() {
    return stateStream;
  }

  Future<void> connect(String btAddress) async {
    await _packetManager.connect(btAddress, onReceived, onClose);

    updateState();

    _pollingTimer = new Timer.periodic(Duration(seconds: 3), (_) {
      updateState();
    });
  }

  void disconnect() {
    _pollingTimer?.cancel();
    _packetManager.disconnect();
  }

  void onClose() {}

  void onReceived(Uint8List data) {
    if (data.length == 0) {
      if (_pingCompleter?.isCompleted == false) {
        _pingCompleter.complete();
      }
    } else {
      if (data[0] == PacketType.Error) {
      } else if (data[0] == PacketType.Event) {
      } else if (data[0] == PacketType.Message) {
      } else {
        Error error = Error.values[(data[0] & 0xC0) >> 6];
        switch (data[0] & 0x3F) {
          //CommandStart
          case PacketType.CommandStart:
            if (_commandStartCompleter?.isCompleted == false) {
              if (error == Error.NoError)
                _commandStartCompleter.complete();
              else
                _commandStartCompleter.completeError(error);
            }
            break;
          //CommandStop
          case PacketType.CommandStart:
            if (_commandStopCompleter?.isCompleted == false) {
              if (error == Error.NoError)
                _commandStopCompleter.complete();
              else
                _commandStopCompleter.completeError(error);
            }
            break;
          //Get default options
          case PacketType.GetDefaultOptions:
            if (_getDefaultParamsCompleter?.isCompleted == false) {
              if (error == Error.NoError) {
                var options = parceOptions(data);
                _getDefaultParamsCompleter.complete(options);
              } else
                _getDefaultParamsCompleter.completeError(error);
            }
            break;
          //Get Status
          case PacketType.GetStatus:
            if (_stateCompleter?.isCompleted == false) {
              if (error == Error.NoError) {
                var status = parceState(data);
                _stateCompleter.complete(status);
              } else
                _getDefaultParamsCompleter.completeError(error);
            }
            break;
        }
      }
    }
  }

  Future<void> updateState() async {
    var data = Uint8List(1);

    ByteData.view(data.buffer)..setUint8(0, PacketType.GetStatus);

    _packetManager.send(data);

    _stateCompleter = Completer<WasherState>();
    WasherState state = await _stateCompleter.future;

    if (stateStreamController?.isClosed == false)
      stateStreamController.add(state);
  }

  Future<Params> getDefaultParams(Program program) {
    var data = Uint8List(2);

    ByteData.view(data.buffer)
      ..setUint8(0, PacketType.GetDefaultOptions)
      ..setUint8(1, program.index);

    _packetManager.send(data);

    _getDefaultParamsCompleter = Completer<Params>();
    return _getDefaultParamsCompleter.future;
  }

  Future<void> startProgram(Program program, Params params) {
    var data = Uint8List(8);

    ByteData.view(data.buffer)
      ..setUint8(0, PacketType.CommandStart)
      ..setUint8(1, program.index)
      ..setUint8(2, params.temperature != null ? params.temperature : 0xFF)
      ..setUint8(3, params.duration != null ? params.duration : 0xFF)
      ..setUint8(4, params.washingSpeed != null ? params.washingSpeed : 0xFF)
      ..setUint8(5, params.spinningSpeed != null ? params.spinningSpeed : 0xFF)
      ..setUint8(6, params.waterLevel != null ? params.waterLevel : 0xFF)
      ..setUint8(7, params.rinsingCycles != null ? params.rinsingCycles : 0xFF);

    _packetManager.send(data);

    _startProgramCompleter = Completer<void>();
    return _startProgramCompleter.future;
  }

  Future<void> stopProgram() {
    var data = Uint8List(1);

    ByteData.view(data.buffer)..setUint8(0, PacketType.CommandStop);

    _packetManager.send(data);

    _commandStopCompleter = Completer<void>();
    return _commandStopCompleter.future;
  }

  WasherState parceState(Uint8List data) {
    if (data.length != 5 && data.length != 13) {
      throw Exception('incorrect state packet length: $data.length');
    }

    //data[0]== PacketType.GetStatus

    var view = ByteData.view(data.buffer);

    var state = new WasherState();

    state.isRunning = view.getUint8(1) != 0xFF;
    state.program =
        view.getUint8(1) == 0xFF ? null : Program.values[view.getUint8(1)];
    state.stage = Stage.values[view.getUint8(2)];
    state.temperature = view.getInt8(3);
    state.speed = view.getUint8(4);

    if (data.length == 13) {
      state.programFullTime = view.getUint32(5, Endian.little);
      state.programPastTime = view.getUint32(9, Endian.little);
    }

    return state;
  }

  Params parceOptions(Uint8List data) {
    if (data.length != 7) {
      throw Exception('incorrect defaultParams packet length: $data.length');
    }

    var view = ByteData.view(data.buffer);

    var params = new Params();

    params.temperature = view.getUint8(1) == 0xFF ? null : view.getUint8(1);
    params.duration = view.getUint8(2) == 0xFF ? null : view.getUint8(2);
    params.washingSpeed = view.getUint8(3) == 0xFF ? null : view.getUint8(3);
    params.spinningSpeed = view.getUint8(4) == 0xFF ? null : view.getUint8(4);
    params.waterLevel = view.getUint8(5) == 0xFF ? null : view.getUint8(5);
    params.rinsingCycles = view.getUint8(6) == 0xFF ? null : view.getUint8(6);

    return params;
  }
}

class Params {
  int temperature;
  int duration;
  int washingSpeed;
  int spinningSpeed;
  int rinsingCycles;
  int waterLevel;
}

enum Error { NoError, UnsuppertedCommand, BadArgs, Busy }
