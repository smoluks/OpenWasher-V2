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
  bool _isConnecting = false;

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
/*     FlutterBluetoothSerial.instance
        .getBondedDevices()
        .then((List<BluetoothDevice> bondedDevices) {
      setState(() {
        devices = bondedDevices.map((device) => SlaveDevice(device)).toList();
      });
    }); */
  }

  //-----Destructor-----
  @override
  void dispose() {
    _discoveryStreamSubscription.cancel();
    FlutterBluetoothSerial.instance.setPairingRequestHandler(null);
    _discoveringTimer?.cancel();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    if (_isConnecting)
      return new Center(
        child: new CircularProgressIndicator(),
      );
    else
      return Scaffold(
        appBar: AppBar(
          title: Text('Devices'),
        ),
        body: Container(
            child: ListView.builder(
                padding: const EdgeInsets.all(16.0),
                itemCount: devices.length,
                itemBuilder: /*1*/ (BuildContext context, int index) {
                  return BluetoothDeviceListEntry(
                    slaveDevice: devices[index],
                    onTap: () {
                      onTapDevice(devices[index]);
                    },
                  );
                })),
      );
  }

  Future onTapDevice(SlaveDevice slaveDevice) async {
    try {
      if (slaveDevice.device.isBonded) {
        Navigator.of(context).pop(slaveDevice.device);
        return;
      }

      setState(() {
        _isConnecting = true;
      });

      bool bonded = await FlutterBluetoothSerial.instance
          .bondDeviceAtAddress(slaveDevice.device.address);

      if (bonded) {
        Navigator.of(context).pop(slaveDevice.device);
      }
    } catch (ex) {
      showDialogWrapper('Error occured while bonding', "${ex.toString()}", () {
        Navigator.of(context).pop();
        FlutterBluetoothSerial.instance.setPairingRequestHandler(null);
      });
    } finally {
      setState(() {
        _isConnecting = false;
      });
    }
  }

  void showDialogWrapper(String title, String text, Function() onPressed) {
    showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          title: Text(title),
          content: Text(text),
          actions: <Widget>[
            new FlatButton(child: new Text("Close"), onPressed: onPressed),
          ],
        );
      },
    );
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
      _discoveringTimer = Timer.periodic(Duration(seconds: 3), (Timer timer) {
        _startDiscovery();
      });
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
    print("Found device: " +
        (result.device.name ?? "null") +
        "/" +
        (result.device.address ?? "null") +
        ", RSSI: " +
        (result.rssi?.toString() ?? "null") +
        ", " +
        (result.device.bondState?.toString() ?? "null"));

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
          leading: getRSSIIcon(slaveDevice.rssi),
          title: Text(slaveDevice.device.name ?? "Unknown"),
          subtitle: Text(slaveDevice.device.address.toString()),
          trailing: Row(mainAxisSize: MainAxisSize.min, children: <Widget>[
            slaveDevice.device.isConnected
                ? Icon(Icons.import_export)
                : Container(width: 0, height: 0),
            slaveDevice.device.isBonded
                ? Icon(Icons.link)
                : Container(width: 0, height: 0),
          ]),
        );

  static getRSSIIcon(int rssi) {
    return Container(
      margin: new EdgeInsets.all(0),
      child: DefaultTextStyle(
          style: new TextStyle(color: getRSSIColor(rssi)),
          child: Column(mainAxisSize: MainAxisSize.min, children: <Widget>[
            Text(rssi?.toString() ?? "-"),
            Text('dBm'),
          ])),
    );
  }

  static getRSSIColor(int rssi) {
    if (rssi == null) return Colors.red;

    if (rssi >= -35)
      return Colors.greenAccent[700];
    else if (rssi >= -45)
      return Color.lerp(
          Colors.greenAccent[700], Colors.lightGreen, -(rssi + 35) / 10);
    else if (rssi >= -55)
      return Color.lerp(Colors.lightGreen, Colors.lime[600], -(rssi + 45) / 10);
    else if (rssi >= -65)
      return Color.lerp(Colors.lime[600], Colors.amber, -(rssi + 55) / 10);
    else if (rssi >= -75)
      return Color.lerp(
          Colors.amber, Colors.deepOrangeAccent, -(rssi + 65) / 10);
    else if (rssi >= -85)
      return Color.lerp(
          Colors.deepOrangeAccent, Colors.redAccent, -(rssi + 75) / 10);
    else
      return Colors.redAccent;
  }
}
