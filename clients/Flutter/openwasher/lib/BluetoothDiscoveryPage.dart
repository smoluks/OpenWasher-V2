import 'dart:async';
import 'package:flutter/material.dart';
import 'package:flutter_bluetooth_serial/flutter_bluetooth_serial.dart';

import 'Dto/OpenWasherDevice.dart';

class BluetoothDeviceListEntry {
  BluetoothDevice device;
  int rssi;

  BluetoothDeviceListEntry(this.device, this.rssi);
}

class BluetoothDiscoveryPage extends StatefulWidget {
  const BluetoothDiscoveryPage();

  @override
  _BluetoothDiscoveryPage createState() => new _BluetoothDiscoveryPage();
}

class _BluetoothDiscoveryPage extends State<BluetoothDiscoveryPage> {
  StreamSubscription<BluetoothDiscoveryResult> _streamSubscription;
  List<BluetoothDeviceListEntry> results = List<BluetoothDeviceListEntry>();
  bool _isDiscovering = false;
  bool _isBonding = false;

  @override
  void initState() {
    super.initState();

    //add all bonded
    FlutterBluetoothSerial.instance
        .getBondedDevices()
        .then((List<BluetoothDevice> bondedDevices) {
      bondedDevices.forEach((device) => _addDevice(device, null));
    });

    _startDiscovery();
  }

  @override
  void dispose() {
    // Avoid memory leak (`setState` after dispose) and cancel discovery
    _streamSubscription?.cancel();

    super.dispose();
  }

  void _startDiscovery() {
    setState(() {
      _isDiscovering = true;
    });

    try {
      _streamSubscription =
          FlutterBluetoothSerial.instance.startDiscovery().listen((result) {
        _addDevice(result.device, result.rssi);
      });
    } catch (ex) {
      print('startDiscovery failed: $ex');
      Navigator.of(context).pop(null);
    }

    _streamSubscription.onDone(() {
      setState(() {
        _isDiscovering = false;
      });
    });
  }

  void _addDevice(BluetoothDevice device, int rssi) {
    print("Found device: " +
        (device.name ?? "null") +
        "/" +
        (device.address ?? "null") +
        ", RSSI: " +
        (rssi?.toString() ?? "null") +
        ", " +
        (device.bondState?.toString() ?? "null"));

    setState(() {
      var entry = results.singleWhere(
          (entry) => entry.device.address == device.address,
          orElse: () => null);

      if (entry != null) {
        entry.rssi = rssi;
      } else {
        results.add(new BluetoothDeviceListEntry(device, rssi));
      }
    });
  }

  onTap(BluetoothDeviceListEntry f) {
    if (f.device.isBonded) {
      Navigator.of(context)
          .pop(new OpenWasherDevice(f.device.address, f.device.name));
      return;
    }

    if (_isBonding) {
      return;
    }

    showGlobalSpinner("Bonding...");
    _isBonding = true;

    FlutterBluetoothSerial.instance.bondDeviceAtAddress(f.device.address).then(
        (bonded) {
      hideGlobalSpinner();
      _isBonding = false;

      if (bonded) {
        Navigator.of(context)
            .pop(new OpenWasherDevice(f.device.address, f.device.name));
      }
    }, onError: (error) {
      hideGlobalSpinner();
      _isBonding = false;

      print("Error occured while bonding : ${error.toString()}");
    });
  }

  void showGlobalSpinner(String text) {
    showDialog(
      context: context,
      barrierDismissible: false,
      builder: (BuildContext context) {
        return WillPopScope(
          onWillPop: () {},
          child: new AlertDialog(
            contentPadding: EdgeInsets.only(
              top: 20,
              bottom: 20,
              left: 20,
              right: 20,
            ),
            // decoration: new BoxDecoration(
            //   color: Colors.white,
            //   shape: BoxShape.rectangle,
            //   borderRadius: BorderRadius.circular(4),
            //   boxShadow: [
            //     BoxShadow(
            //       color: Colors.black26,
            //       blurRadius: 10.0,
            //       offset: const Offset(0.0, 10.0),
            //     ),
            //   ],
            // ),
            content: new Row(
              children: <Widget>[
                new CircularProgressIndicator(),
                new Container(width: 15, height: 0),
                new Text(text),
              ],
            ),
            actions: <Widget>[],
          ),
        );
      },
    );
  }

  void hideGlobalSpinner() {
    Navigator.of(context).pop();
  }

  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Select device'),
        actions: <Widget>[
          _isDiscovering
              ? FittedBox(
                  child: Container(
                    margin: new EdgeInsets.all(16.0),
                    child: CircularProgressIndicator(
                      valueColor: AlwaysStoppedAnimation<Color>(
                        Colors.white,
                      ),
                    ),
                  ),
                )
              : IconButton(
                  icon: Icon(Icons.replay),
                  onPressed: _startDiscovery,
                )
        ],
      ),
      body: ListView(children: results.map((f) => _getView(f)).toList()),
    );
  }

  ListTile _getView(BluetoothDeviceListEntry f) {
    return new ListTile(
        onTap: () {
          onTap(f);
        },
        //onLongPress: onLongPress,
        //enabled: enabled,
        leading:
            Icon(Icons.devices), // @TODO . !BluetoothClass! class aware icon
        title: Text(f.device.name ?? "Unknown device"),
        subtitle: Text(f.device.address.toString()),
        trailing: Row(
          mainAxisSize: MainAxisSize.min,
          children: <Widget>[
            f.device.isConnected
                ? Icon(Icons.import_export)
                : Container(width: 0, height: 0),
            f.device.isBonded
                ? Icon(Icons.link)
                : Container(width: 0, height: 0),
            f.rssi != null
                ? Container(
                    margin: new EdgeInsets.all(8.0),
                    child: DefaultTextStyle(
                      style: _computeTextStyle(f.rssi),
                      child: Column(
                        mainAxisSize: MainAxisSize.min,
                        children: <Widget>[
                          Text(f.rssi.toString()),
                          Text('dBm'),
                        ],
                      ),
                    ),
                  )
                : Container(width: 0, height: 0),
          ],
        ));
  }

  static TextStyle _computeTextStyle(int rssi) {
    /**/ if (rssi >= -35)
      return TextStyle(color: Colors.greenAccent[700]);
    else if (rssi >= -45)
      return TextStyle(
          color: Color.lerp(
              Colors.greenAccent[700], Colors.lightGreen, -(rssi + 35) / 10));
    else if (rssi >= -55)
      return TextStyle(
          color: Color.lerp(
              Colors.lightGreen, Colors.lime[600], -(rssi + 45) / 10));
    else if (rssi >= -65)
      return TextStyle(
          color: Color.lerp(Colors.lime[600], Colors.amber, -(rssi + 55) / 10));
    else if (rssi >= -75)
      return TextStyle(
          color: Color.lerp(
              Colors.amber, Colors.deepOrangeAccent, -(rssi + 65) / 10));
    else if (rssi >= -85)
      return TextStyle(
          color: Color.lerp(
              Colors.deepOrangeAccent, Colors.redAccent, -(rssi + 75) / 10));
    else
      /*code symetry*/
      return TextStyle(color: Colors.redAccent);
  }
}
