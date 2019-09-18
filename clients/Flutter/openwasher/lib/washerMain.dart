import 'package:flutter/material.dart';
import 'package:flutter_bluetooth_serial/flutter_bluetooth_serial.dart';
import 'devicesList.dart';

class WasherMain extends StatefulWidget {
  @override
  WasherMainState createState() => WasherMainState();
}

class WasherMainState extends State<WasherMain> {
  BluetoothDevice _selectedDevice;

  @override
  void initState() {
    super.initState();

    init();
  }

  init() async {
    var enabled = await FlutterBluetoothSerial.instance.requestEnable();
    if (!enabled) Navigator.of(context).pop();

    _selectedDevice =
        await Navigator.of(context).push(MaterialPageRoute(builder: (context) {
      return DevicesList();
    }));
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text("Title"),
      ),
      //
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            Text(
              'You make: ',
            ),
            Text(
              _selectedDevice?.name ?? "Not selected",
              style: Theme.of(context).textTheme.display1,
            ),
          ],
        ),
      ),
    );
  }
}
