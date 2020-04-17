class PacketType {
  static const Ping = 0x00;
  static const CommandStart = 0x01;
  static const CommandStop = 0x02;
  static const GetDefaultOptions = 0x03;
  static const EnterProgrammingMode = 0x0A;
  static const GetStatus = 0x10;
  static const GetSetConfig = 0x20;
  static const GetSetTime = 0x30;
  static const Event = 0x5E;
  static const Error = 0x5B;
  static const Message = 0x58;
}
