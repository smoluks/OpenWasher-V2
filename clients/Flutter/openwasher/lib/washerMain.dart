import 'package:flutter/material.dart';
import 'package:flutter_bluetooth_serial/flutter_bluetooth_serial.dart';
import 'devicesList.dart';
import 'openWasherAPI.dart';

class WasherMain extends StatefulWidget {
  @override
  WasherMainState createState() => WasherMainState();
}

class WasherMainState extends State<WasherMain> {
  BluetoothDevice _selectedDevice;
  OpenWasherApi _apiManager;
  String _error;

  @override
  void initState() {
    super.initState();
    init();
  }

  @override
  void dispose() {
    _apiManager?.dispose();
    super.dispose();
  }

  init() async {
    var enabled = await FlutterBluetoothSerial.instance.requestEnable();
    if (!enabled) Navigator.of(context).pop();

    selectDevice();
  }

  selectDevice() async {
    var device =
        await Navigator.of(context).push(MaterialPageRoute(builder: (context) {
      return DevicesList();
    }));

    setState(() {
      _selectedDevice = device;
      if (_selectedDevice == null) return;

      try {
        _apiManager = new OpenWasherApi(_selectedDevice.address);
        _apiManager.ping();
      } catch (ex) {
        _error = ex.toString();
      }
    });
  }

  @override
  Widget build(BuildContext context) {
    if (_apiManager == null)
      return getNotSelectedWiev();
    else if (!_apiManager.isConnected())
      return connectingView();
    else
      return getNotSelectedWiev();
  }

  Widget getNormalView() {
    return Scaffold(
        appBar: AppBar(
          title: Text(_selectedDevice?.name ?? "Unknown"),
        ),
        //
        body: Column(
          children: <Widget>[
            // Expanded(
            //   child: FittedBox(
            //     fit: BoxFit.contain,
            //     child: Image.asset('images/washer_stopped.png'),
            //   ),
            // ),
            Text('Pragram:'),
            Text('Stage:'),
            Text('T:'),
            Text('Error: ' + _error),
          ],
        ));
  }

  Widget connectingView() {
    return Scaffold(
      appBar: AppBar(
        title: Text("Connecting..."),
      ),
      body: Center(),
    );
  }

  Widget getNotSelectedWiev() {
    return Scaffold(
      appBar: AppBar(
        title: Text("No connection"),
      ),
      body: Center(
          child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: <Widget>[
            FlatButton(
              color: Colors.cyan,
              padding: EdgeInsets.all(10.0),
              child: Column(children: [
                Icon(Icons.bluetooth_searching),
                const Text('Search washer', style: TextStyle(fontSize: 20))
              ]),
              onPressed: () {
                selectDevice();
              },
            ),
          ])),
    );
  }
}
