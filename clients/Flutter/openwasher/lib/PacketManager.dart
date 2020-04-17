import 'dart:async';
import 'dart:typed_data';

import 'package:flutter_bluetooth_serial/flutter_bluetooth_serial.dart';

typedef void CloseCallback();
typedef void PacketReceivedCallback(Uint8List data);

enum State { WaitStart, WaitLength, CollectData }

class PacketManager {
  BluetoothConnection _connection;

  CloseCallback _closeCallback;
  PacketReceivedCallback _receivedCallback;

  State _state = State.WaitStart;

  Future<void> connect(String btAddress, PacketReceivedCallback receivedCallback, CloseCallback closeCallback) async {
    _receivedCallback = receivedCallback;
    _closeCallback = closeCallback;

    await BluetoothConnection.toAddress(btAddress).then((connection) {
      print('Connected to the device $btAddress');
      _connection = connection;

      _connection.input.listen(_onData).onDone(() {
        _closeCallback();
      });
    });
  }

  void disconnect() {
    _connection.close();
    _connection.dispose();
  }

  void send(Uint8List data) {
    Uint8List packet = new Uint8List(data.length + 3);
    ByteData.view(packet.buffer)
      ..setUint8(0, 0xAB)
      ..setUint8(1, data.length);

    int crc = getCrc(packet, data.length + 2);
    ByteData.view(packet.buffer)
      ..setUint8(data.length + 2, crc);

    try {
      _connection.output.add(packet);
    } catch (ex) {
      //onError(ex, null);
    }
  }

  int _length;
  Uint8List _packet;

  void _onData(Uint8List data) {
    data.forEach((value) {
      addCrc(value);

      switch (_state) {
        case State.WaitStart:
          if (value == 0xAB) _state = State.WaitLength;
          break;
        case State.WaitLength:
          _length = value;
          _packet = new Uint8List(_length);
          _state = State.CollectData;
          break;
        case State.CollectData:
          if (_packet.length != _length) {
            _packet.add(value);
          } else {
            if (_crc == 0) {
              _receivedCallback(_packet);
            }

            _crc = 0;
          }
          break;
      }
    });
  }

  int _crc = 0;
  void addCrc(int data) {
    for (int bitCounter = 0; bitCounter < 8; bitCounter++) {
      if (((_crc ^ data) & 0x01) != 0) {
        _crc = ((_crc >> 1) ^ 0x8C);
      } else {
        _crc >>= 1;
      }
      data >>= 1;
    }
  }

  int getCrc(Uint8List data, [int count = -1]) {
    if (count == -1) count = data.length;

    int crc = 0;

    for (int index = 0; index < count; index++) {
      var currentByte = data[index];

      for (int bitCounter = 0; bitCounter < 8; bitCounter++) {
        if (((crc ^ currentByte) & 0x01) != 0) {
          crc = ((crc >> 1) ^ 0x8C);
        } else {
          crc >>= 1;
        }
        currentByte >>= 1;
      }
    }

    return crc;
  }
}
