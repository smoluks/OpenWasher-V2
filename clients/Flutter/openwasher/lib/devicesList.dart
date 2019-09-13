import 'package:flutter/material.dart';
import 'package:flutter_bluetooth_serial/flutter_bluetooth_serial.dart';
import 'dart:async';

class SlaveDevice extends BluetoothDevice {
  BluetoothDevice device;
  int rssi;

  SlaveDevice(this.device, [this.rssi]);
}

class DevicesList extends StatefulWidget {
  @override
  DevicesListState createState() => new DevicesListState();
}

class DevicesListState extends State<DevicesList> {
  //Modem state
  BluetoothState _bluetoothState = BluetoothState.UNKNOWN;
  //Discovery subscription
  StreamSubscription<BluetoothDiscoveryResult> _discoveryStreamSubscription;
  //Found device list
  List<SlaveDevice> devices = List<SlaveDevice>();
  //
  Timer _discoveringTimer;
  //
  bool _isDiscovering = false;

  //-----Constructor-----
  @override
  void initState() {
    super.initState();

    // Get current state
    FlutterBluetoothSerial.instance.state.then((state) {
      setBluetoothState(state);
    });

    // Listen for futher state changes
    FlutterBluetoothSerial.instance
        .onStateChanged()
        .listen((BluetoothState state) {
      setBluetoothState(state);
    });

    // Setup a list of the bonded devices
    FlutterBluetoothSerial.instance
        .getBondedDevices()
        .then((List<BluetoothDevice> bondedDevices) {
      setState(() {
        devices = bondedDevices.map((device) => SlaveDevice(device)).toList();
      });
    });
  }

  //-----Destructor-----
  @override
  void dispose() {
    _discoveryStreamSubscription.cancel();
    FlutterBluetoothSerial.instance.setPairingRequestHandler(null);
    _discoveringTimer?.cancel();
    super.dispose();
  }

  void setBluetoothState(BluetoothState state) {
    print("BT state: " + state.stringValue);
    setState(() {
      _bluetoothState = state;
    });

    if (_bluetoothState == BluetoothState.STATE_ON) {
      _startDiscovery();
      //
      _discoveringTimer?.cancel();
      _discoveringTimer =
          Timer.periodic(Duration(seconds: 3), (Timer timer) {});
    } else
      _discoveringTimer?.cancel();
  }

  void _startDiscovery() {
    if (_isDiscovering) return;

    _isDiscovering = true;

    _discoveryStreamSubscription =
        FlutterBluetoothSerial.instance.startDiscovery().listen((r) {
      addOrUpdateDevice(r);
    });

    _discoveryStreamSubscription.onDone(() {
      setState(() {
        _isDiscovering = false;
      });
    });
  }

  void addOrUpdateDevice(BluetoothDiscoveryResult result) {
    print("Found device: " + result.device.address);

    setState(() {
      Iterator i = devices.iterator;
      while (i.moveNext()) {
        var _device = i.current;
        if (_device.device == result.device) {
          _device.rssi = result.rssi;
          return;
        }
      }

      devices.add(SlaveDevice(result.device, result.rssi));
    });
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
                  slaveDevice: devices[index - 2],
                  onTap: () {
                    onTapDevice(devices[index - 2]);
                  },
                );
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

  Future onTapDevice(SlaveDevice slaveDevice) async {
    try {
      if (slaveDevice.device.isBonded) {
        Navigator.of(context).pop(slaveDevice.device);
        return;
      }

      bool bonded = await FlutterBluetoothSerial.instance
          .bondDeviceAtAddress(slaveDevice.device.address);

      if (bonded) {
        Navigator.of(context).pop(slaveDevice.device);
      }
    } catch (ex) {
      showDialog(
        context: context,
        builder: (BuildContext context) {
          return AlertDialog(
            title: const Text('Error occured while bonding'),
            content: Text("${ex.toString()}"),
            actions: <Widget>[
              new FlatButton(
                child: new Text("Close"),
                onPressed: () {
                  Navigator.of(context).pop();
                  FlutterBluetoothSerial.instance
                      .setPairingRequestHandler(null);
                },
              ),
            ],
          );
        },
      );
    }
  }
}

class BluetoothDeviceListEntry extends ListTile {
  BluetoothDeviceListEntry(
      {@required SlaveDevice slaveDevice,
      GestureTapCallback onTap,
      GestureLongPressCallback onLongPress,
      bool enabled = true})
      : super(
          onTap: onTap,
          onLongPress: onLongPress,
          enabled: enabled,
          leading:
              Icon(Icons.devices), // @TODO . !BluetoothClass! class aware icon
          title: Text(slaveDevice.device.name ?? "Unknown device"),
          subtitle: Text(slaveDevice.device.address.toString()),
          trailing: Row(mainAxisSize: MainAxisSize.min, children: <Widget>[
            slaveDevice.rssi != null
                ? Container(
                    margin: new EdgeInsets.all(8.0),
                    child: DefaultTextStyle(
                        style: () {
                          /**/ if (slaveDevice.rssi >= -35)
                            return TextStyle(color: Colors.greenAccent[700]);
                          else if (slaveDevice.rssi >= -45)
                            return TextStyle(
                                color: Color.lerp(
                                    Colors.greenAccent[700],
                                    Colors.lightGreen,
                                    -(slaveDevice.rssi + 35) / 10));
                          else if (slaveDevice.rssi >= -55)
                            return TextStyle(
                                color: Color.lerp(
                                    Colors.lightGreen,
                                    Colors.lime[600],
                                    -(slaveDevice.rssi + 45) / 10));
                          else if (slaveDevice.rssi >= -65)
                            return TextStyle(
                                color: Color.lerp(
                                    Colors.lime[600],
                                    Colors.amber,
                                    -(slaveDevice.rssi + 55) / 10));
                          else if (slaveDevice.rssi >= -75)
                            return TextStyle(
                                color: Color.lerp(
                                    Colors.amber,
                                    Colors.deepOrangeAccent,
                                    -(slaveDevice.rssi + 65) / 10));
                          else if (slaveDevice.rssi >= -85)
                            return TextStyle(
                                color: Color.lerp(
                                    Colors.deepOrangeAccent,
                                    Colors.redAccent,
                                    -(slaveDevice.rssi + 75) / 10));
                          else
                            /*code symetry*/
                            return TextStyle(color: Colors.redAccent);
                        }(),
                        child: Column(
                            mainAxisSize: MainAxisSize.min,
                            children: <Widget>[
                              Text(slaveDevice.rssi.toString()),
                              Text('dBm'),
                            ])),
                  )
                : Container(width: 0, height: 0),
            slaveDevice.device.isConnected
                ? Icon(Icons.import_export)
                : Container(width: 0, height: 0),
            slaveDevice.device.isBonded
                ? Icon(Icons.link)
                : Container(width: 0, height: 0),
          ]),
        );
}
