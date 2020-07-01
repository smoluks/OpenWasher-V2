import 'dart:async';

import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';
import 'package:flutter_i18n/flutter_i18n.dart';
import 'package:openwasher/Dto/Program.dart';

import 'Dto/WasherState.dart';
import 'Helpers/Dialogs.dart';
import 'Managers/ProtocolManager.dart';
import 'StatusPage.dart';

class ParametersPage extends StatefulWidget {
  final Program program;

  const ParametersPage(this.program);

  @override
  _ParametersPage createState() => new _ParametersPage();
}

class _ParametersPage extends State<ParametersPage> {
  bool _loading = true;
  Params _params;
  StreamSubscription<WasherState> _stateSubscription;

  @override
  void initState() {
    print('ParametersPage creating...');

    super.initState();

    getDefaultParams();

    _stateSubscription =
        ProtocolManager.instance.onStateChange().listen(onStateChange);
  }

  Future<void> getDefaultParams() async {
    try {
      var params =
          await ProtocolManager.instance.getDefaultParams(widget.program);

      setState(() {
        _loading = false;
        this._params = params;
      });
    } catch (ex) {
      print('getDefaultParams error: $ex');

      await Dialogs.showErrorBox(
          context, 'GetDefaultParams error: ${ex.message}', () {
        Navigator.of(context).pop(false);
      });
    }
  }

  Future<void> onStateChange(WasherState state) async {
    if (state.isRunning) {
      await _stateSubscription?.cancel();

      await Navigator.of(context).push(MaterialPageRoute(builder: (context) {
        return StatusPage(state);
      }));

      Navigator.of(context).pop(null);
    }
  }

  @override
  void dispose() {
    print('ParametersPage destroying...');

    _stateSubscription?.cancel();

    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(FlutterI18n.translate(
            context, "program.${widget.program.index}.name")),
        actions: <Widget>[
          _loading
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
              : Icon(Icons.bluetooth_connected)
        ],
      ),
      body: Container(
        child: new ListView(children: getParamsListTiles()),
      ),
    );
  }

  getParamsListTiles() {
    bool hasCustomParams = false;
    var result = new List<Widget>();

    if (_loading) return result;

    if (_params.temperature != null) {
      result.add(getTemperatureSelector());
      hasCustomParams = true;
    }
    if (_params.duration != null) {
      result.add(getDurationSelector());
      hasCustomParams = true;
    }
    if (_params.washingSpeed != null) {
      result.add(getWashingSpeedSelector());
      hasCustomParams = true;
    }
    if (_params.spinningSpeed != null) {
      result.add(getSpinningSpeedSelector());
      hasCustomParams = true;
    }
    if (_params.rinsingCycles != null) {
      result.add(getRinsingCyclesSelector());
      hasCustomParams = true;
    }
    if (_params.waterLevel != null) {
      result.add(getWaterLevelSelector());
      hasCustomParams = true;
    }

    if (!hasCustomParams) {
      result.add(getNoParamsText());
    }

    result.add(getButton());

    return result;
  }

  Widget getTemperatureSelector() {
    return ListTile(
      enabled: _params.temperature != null,
      subtitle: Text("Temperature ${_params.temperature}°"),
      title: Container(
        alignment: Alignment.bottomCenter,
        child: Slider(
            value: _params.temperature == null
                ? 30
                : _params.temperature.toDouble(),
            min: 30.0,
            max: 80.0,
            divisions: 10,
            activeColor:
                _params.temperature != null ? Colors.blue : Colors.grey,
            inactiveColor:
                _params.temperature != null ? Colors.black : Colors.grey,
            label: _params.temperature.toString(),
            onChanged: (double newValue) {
              if (_params.temperature != null) {
                setState(() {
                  _params.temperature = newValue.round();
                });
              }
            },
            semanticFormatterCallback: (double newValue) {
              return '${newValue.round()} dollars';
            }),
      ),
    );
  }

  Widget getDurationSelector() {
    return ListTile(
      enabled: _params.duration != null,
      subtitle: Text("Duration  ${_params.duration} min."),
      title: Container(
        alignment: Alignment.bottomCenter,
        child: Slider(
            value: _params.duration == null ? 15 : _params.duration.toDouble(),
            min: 15.0,
            max: 180.0,
            divisions: 165,
            activeColor: _params.duration != null ? Colors.blue : Colors.grey,
            inactiveColor:
                _params.duration != null ? Colors.black : Colors.grey,
            label: _params.duration.toString(),
            onChanged: (double newValue) {
              if (_params.duration != null) {
                setState(() {
                  _params.duration = newValue.round();
                });
              }
            },
            semanticFormatterCallback: (double newValue) {
              return '${newValue.round()} dollars';
            }),
      ),
    );
  }

  Widget getWashingSpeedSelector() {
    return ListTile(
      enabled: _params.washingSpeed != null,
      subtitle: Text("Washing speed  ${_params.washingSpeed} rps."),
      title: Container(
        alignment: Alignment.bottomCenter,
        child: Slider(
            value: _params.washingSpeed == null
                ? 3
                : _params.washingSpeed.toDouble(),
            min: 1.0,
            max: 5.0,
            divisions: 4,
            activeColor:
                _params.washingSpeed != null ? Colors.blue : Colors.grey,
            inactiveColor:
                _params.washingSpeed != null ? Colors.black : Colors.grey,
            label: _params.washingSpeed.toString(),
            onChanged: (double newValue) {
              if (_params.washingSpeed != null) {
                setState(() {
                  _params.washingSpeed = newValue.round();
                });
              }
            },
            semanticFormatterCallback: (double newValue) {
              return '${newValue.round()} dollars';
            }),
      ),
    );
  }

  Widget getSpinningSpeedSelector() {
    return ListTile(
      enabled: _params.spinningSpeed != null,
      subtitle: Text("Spinning speed  ${_params.spinningSpeed} rps."),
      title: Container(
        alignment: Alignment.bottomCenter,
        child: Slider(
            value: _params.spinningSpeed == null
                ? 3
                : _params.spinningSpeed.toDouble(),
            min: 0.0,
            max: 5.0,
            divisions: 5,
            activeColor:
                _params.spinningSpeed != null ? Colors.blue : Colors.grey,
            inactiveColor:
                _params.spinningSpeed != null ? Colors.black : Colors.grey,
            label: _params.spinningSpeed.toString(),
            onChanged: (double newValue) {
              if (_params.spinningSpeed != null) {
                setState(() {
                  _params.spinningSpeed = newValue.round();
                });
              }
            },
            semanticFormatterCallback: (double newValue) {
              return '${newValue.round()} dollars';
            }),
      ),
    );
  }

  Widget getRinsingCyclesSelector() {
    return ListTile(
      enabled: _params.rinsingCycles != null,
      subtitle: Text("Rinsing cycles ${_params.rinsingCycles}."),
      title: Container(
        alignment: Alignment.bottomCenter,
        child: Slider(
            value: _params.rinsingCycles == null
                ? 3
                : _params.rinsingCycles.toDouble(),
            min: 0.0,
            max: 5.0,
            divisions: 4,
            activeColor:
                _params.rinsingCycles != null ? Colors.blue : Colors.grey,
            inactiveColor:
                _params.rinsingCycles != null ? Colors.black : Colors.grey,
            label: _params.rinsingCycles.toString(),
            onChanged: (double newValue) {
              if (_params.rinsingCycles != null) {
                setState(() {
                  _params.rinsingCycles = newValue.round();
                });
              }
            },
            semanticFormatterCallback: (double newValue) {
              return '${newValue.round()} dollars';
            }),
      ),
    );
  }

  Widget getWaterLevelSelector() {
    return ListTile(
      enabled: _params.waterLevel != null,
      subtitle: Text("Water level ${_params.waterLevel} %."),
      title: Container(
        alignment: Alignment.bottomCenter,
        child: new Container(
            //margin: const EdgeInsets.all(15.0),
            //padding: const EdgeInsets.all(3.0),
            decoration: new BoxDecoration(
                border: new Border.all(color: Colors.blueAccent)),
            child: Slider(
                value: _params.waterLevel == null
                    ? 3
                    : _params.waterLevel.toDouble(),
                min: 0.0,
                max: 100.0,
                divisions: 100,
                activeColor:
                    _params.waterLevel != null ? Colors.blue : Colors.grey,
                inactiveColor:
                    _params.waterLevel != null ? Colors.black : Colors.grey,
                label: _params.waterLevel.toString(),
                onChanged: (double newValue) {
                  if (_params.waterLevel != null) {
                    setState(() {
                      _params.waterLevel = newValue.round();
                    });
                  }
                },
                semanticFormatterCallback: (double newValue) {
                  return '${newValue.round()} dollars';
                })),
      ),
    );
  }

  Widget getNoParamsText() {
    return ListTile(
        title: Text(
      "No configurable parameters",
      style: TextStyle(fontSize: 16),
    ));
  }

  Widget getButton() {
    return ListTile(
        enabled: _params != null,
        title: RaisedButton(
          shape: new RoundedRectangleBorder(
              borderRadius: new BorderRadius.circular(18.0),
              side: BorderSide(color: Colors.blue)),
          color: Colors.blue,
          textColor: Colors.white,
          padding: EdgeInsets.all(8.0),
          onPressed: () async {
            setState(() {
              _loading = true;
            });

            try {
              await ProtocolManager.instance
                  .startProgram(widget.program, _params);

              ProtocolManager.instance.updateState();
            } catch (ex) {
              print('start error: $ex');
              await Dialogs.showErrorBox(
                  context, 'Start error: ${ex.message}', null);
            }

            setState(() {
              _loading = false;
            });
          },
          child: Text(
            "Start",
            style: TextStyle(
              fontSize: 20.0,
            ),
          ),
        ));
  }
}
