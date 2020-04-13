import 'dart:typed_data';

import 'package:flutter_bluetooth_serial/flutter_bluetooth_serial.dart';

typedef void CloseCallback();

enum State { WaitStart, WaitLength, CollectData }

class PacketManager {
  BluetoothConnection _connection;
  CloseCallback _closeCallback;
  State _state = State.WaitStart;

  Future<void> connect(String btAddress, CloseCallback closeCallback) async {
    _closeCallback = closeCallback;

    await BluetoothConnection.toAddress(btAddress).then((connection) {
      print('Connected to the device $btAddress');
      _connection = connection;

      _connection.input.listen(_onData).onDone(() {
        _closeCallback();
      });
    });
  }

  int _length;
  Uint8List _packet;

  void _onData(Uint8List data) {
    data.forEach((element) {
      switch (_state) {
        case State.WaitStart:
          if (element == 0xAB) _state = State.WaitLength;
          break;
        case State.WaitLength:
          _length = element;
          _packet = new Uint8List(_length);
          _state = State.CollectData;
          break;
        case State.CollectData:

          break;
      }
    });
  }
}
