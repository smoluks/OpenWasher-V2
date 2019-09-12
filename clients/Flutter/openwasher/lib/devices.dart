import 'package:flutter/material.dart';
import 'package:flutter_bluetooth_serial/flutter_bluetooth_serial.dart';
import 'dart:async';

enum _SlaveState { Unknown, Bonded }

class _Slave extends BluetoothDevice {
  BluetoothDevice device;
  _SlaveState state = _SlaveState.Unknown;
  int rssi;

  _Slave(this.device, [this.rssi]);
}

class DevicesListState extends State<DevicesList> {
  //Modem state
  BluetoothState _bluetoothState = BluetoothState.UNKNOWN;
  //Discovery subscription
  StreamSubscription<BluetoothDiscoveryResult> _discoveryStreamSubscription;
  //Found device list
  List<_Slave> devices = List<_Slave>();
  Timer _discoverableTimeoutTimer;

  @override
  void initState() {
    super.initState();

    // Get current state
    FlutterBluetoothSerial.instance.state.then((state) {
      setState(() {
        _bluetoothState = state;
      });
    });

    // Listen for futher state changes
    FlutterBluetoothSerial.instance
        .onStateChanged()
        .listen((BluetoothState state) {
      setState(() {
        _bluetoothState = state;

        if (_bluetoothState == BluetoothState.STATE_ON) {
          _startDiscovery();
          //
          _discoverableTimeoutTimer?.cancel();
          _discoverableTimeoutTimer =
              Timer.periodic(Duration(seconds: 3), (Timer timer) {
            _startDiscovery();
          });
        } else {
          // Discoverable mode is disabled when Bluetooth gets disabled
          _discoverableTimeoutTimer = null;
        }
      });
    });

    // Setup a list of the bonded devices
    FlutterBluetoothSerial.instance
        .getBondedDevices()
        .then((List<BluetoothDevice> bondedDevices) {
      setState(() {
        devices = bondedDevices.map((device) => _Slave(device)).toList();
      });
    });

    if (_bluetoothState == BluetoothState.STATE_ON) {
      _startDiscovery();
      //
      _discoverableTimeoutTimer?.cancel();
      _discoverableTimeoutTimer =
          Timer.periodic(Duration(seconds: 3), (Timer timer) {
        _startDiscovery();
      });
    }
  }

  @override
  void dispose() {
    FlutterBluetoothSerial.instance.setPairingRequestHandler(null);
    //_collectingTask?.dispose();
    _discoverableTimeoutTimer?.cancel();
    super.dispose();
  }

  void _startDiscovery() {
    _discoveryStreamSubscription =
        FlutterBluetoothSerial.instance.startDiscovery().listen((r) {
      setState(() {
        addOrUpdateDevice(r);
      });
    });

    _discoveryStreamSubscription.onDone(() {
      setState(() {
        //_isDiscovering = false;
      });
    });
  }

  void addOrUpdateDevice(BluetoothDiscoveryResult result) {
    Iterator i = devices.iterator;
    while (i.moveNext()) {
      var _device = i.current;
      if (_device.device == result.device) {
        _device.state = _SlaveState.Bonded;
        _device.rssi = result.rssi;
        return;
      }
    }

    devices.add(_Slave(result.device, result.rssi));
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Devices'),
      ),
      body: Container(
          child: ListView.builder(
              padding: const EdgeInsets.all(16.0),
              itemCount: devices.length + 2,
              itemBuilder: /*1*/ (BuildContext context, int index) {
                if (index == 0) return btEnableWidget();
                if (index == 1) return Divider();
                return BluetoothDeviceListEntry(
                    device: devices[index - 2].device,
                    rssi: devices[index - 2].rssi);
              })),
    );
  }

  Widget btEnableWidget() {
    return SwitchListTile(
      title: const Text('Enable Bluetooth'),
      value: _bluetoothState.isEnabled,
      onChanged: (bool value) {
        // Do the request and update with the true value then
        future() async {
          // async lambda seems to not working
          if (value)
            await FlutterBluetoothSerial.instance.requestEnable();
          else
            await FlutterBluetoothSerial.instance.requestDisable();
        }

        future().then((_) {
          setState(() {});
        });
      },
    );
  }
}

class BluetoothDeviceListEntry extends ListTile {
  BluetoothDeviceListEntry(
      {@required BluetoothDevice device,
      int rssi,
      GestureTapCallback onTap,
      GestureLongPressCallback onLongPress,
      bool enabled = true})
      : super(
          onTap: onTap,
          onLongPress: onLongPress,
          enabled: enabled,
          leading:
              Icon(Icons.devices), // @TODO . !BluetoothClass! class aware icon
          title: Text(device.name ?? "Unknown device"),
          subtitle: Text(device.address.toString()),
          trailing: Row(mainAxisSize: MainAxisSize.min, children: <Widget>[
            rssi != null
                ? Container(
                    margin: new EdgeInsets.all(8.0),
                    child: DefaultTextStyle(
                        style: () {
                          /**/ if (rssi >= -35)
                            return TextStyle(color: Colors.greenAccent[700]);
                          else if (rssi >= -45)
                            return TextStyle(
                                color: Color.lerp(Colors.greenAccent[700],
                                    Colors.lightGreen, -(rssi + 35) / 10));
                          else if (rssi >= -55)
                            return TextStyle(
                                color: Color.lerp(Colors.lightGreen,
                                    Colors.lime[600], -(rssi + 45) / 10));
                          else if (rssi >= -65)
                            return TextStyle(
                                color: Color.lerp(Colors.lime[600],
                                    Colors.amber, -(rssi + 55) / 10));
                          else if (rssi >= -75)
                            return TextStyle(
                                color: Color.lerp(
                                    Colors.amber,
                                    Colors.deepOrangeAccent,
                                    -(rssi + 65) / 10));
                          else if (rssi >= -85)
                            return TextStyle(
                                color: Color.lerp(Colors.deepOrangeAccent,
                                    Colors.redAccent, -(rssi + 75) / 10));
                          else
                            /*code symetry*/
                            return TextStyle(color: Colors.redAccent);
                        }(),
                        child: Column(
                            mainAxisSize: MainAxisSize.min,
                            children: <Widget>[
                              Text(rssi.toString()),
                              Text('dBm'),
                            ])),
                  )
                : Container(width: 0, height: 0),
            device.isConnected
                ? Icon(Icons.import_export)
                : Container(width: 0, height: 0),
            device.isBonded ? Icon(Icons.link) : Container(width: 0, height: 0),
          ]),
        );
}

class DevicesList extends StatefulWidget {
  @override
  DevicesListState createState() => new DevicesListState();
}
