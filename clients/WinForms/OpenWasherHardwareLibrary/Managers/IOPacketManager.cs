using OpenWasherHardwareLibrary.Enums;
using OpenWasherHardwareLibrary.Exceptions;
using OpenWasherHardwareLibrary.Managers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static OpenWasherHardwareLibrary.HardwareLibrary;

namespace OpenWasherHardwareLibrary
{
    internal class IOManager : IDisposable
    {
        const int baudrate = 115200;
        const Parity parity = Parity.Even;
        const int IO_TIMEOUT = 500;
        const int READ_GAP = 200;

        private readonly MessageReceivedDelegate messageDelegate;
        private readonly ErrorReceivedDelegate errorDelegate;
        private readonly EventReceivedDelegate eventDelegate;

        internal string Port { get; private set; }

        readonly SerialPort _serialPort;
        readonly Queue<byte> receiveQueue = new Queue<byte>();
        private readonly ConcurrentDictionary<PacketType, byte[]> waitingAnswers = new ConcurrentDictionary<PacketType, byte[]>();

        internal IOManager(string portname,
            MessageReceivedDelegate messageDelegate = null,
            ErrorReceivedDelegate errorDelegate = null,
            EventReceivedDelegate eventDelegate = null)
        {
            this.messageDelegate = messageDelegate;
            this.errorDelegate = errorDelegate;
            this.eventDelegate = eventDelegate;
            Port = portname;

            _serialPort = new SerialPort
            {
                PortName = portname,
                BaudRate = baudrate,
                Parity = parity,
                ReadTimeout = IO_TIMEOUT,
                WriteTimeout = IO_TIMEOUT
            };

            try
            {
                _serialPort.Open();
                _serialPort.DataReceived += DataReceivedHandler;
            }
            catch (Exception e)
            {
                _serialPort.Close();
                LogManager.WriteException($"{_serialPort.PortName} Open exception:", e);
                throw;
            }
        }

        public void Dispose()
        {
            if (_serialPort == null)
                return;

            if (_serialPort.IsOpen)
            {
                //close may froze
                Task.WhenAny(Task.Run(() => _serialPort.Close()), Task.Delay(3000)).Wait();                
            }

            _serialPort.Dispose();
        }

        ~IOManager()
        {
            if (_serialPort == null)
                return;

            _serialPort.Close();
        }

        internal static IEnumerable<string> GetAvaliableComPorts()
        {
            return SerialPort.GetPortNames();
        }

        internal Task<byte[]> SendAsync(CancellationToken token, byte[] buffer, int timeout)
        {
             return Task.Run(() =>
            {
                var array = new byte[buffer.Count() + 3];
                array[0] = 0xAB;
                array[1] = (byte)buffer.Count();
                buffer.CopyTo(array, 2);
                array[buffer.Count() + 2] = GetCrc(array, 0, buffer.Count() + 2);

                LogManager.WriteData($"{_serialPort.PortName} Send: ", array);

                //send packet
                try
                {
                    _serialPort.Write(array, 0, buffer.Count() + 3);
                }
                catch (Exception e)
                {
                    LogManager.WriteException($"{_serialPort.PortName} Send exception:", e);
                    throw;
                }

                //add waiter
                var type = GetMessageType(buffer);
                waitingAnswers.AddOrUpdate(type, (key) => null, (key, oldValue) => null);

                //check waiter
                timeout = (int)Math.Ceiling((float)timeout / READ_GAP);
                while (timeout-- > 0)
                {
                    if (token.WaitHandle.WaitOne(READ_GAP))
                        throw new OperationCanceledException();

                    if (waitingAnswers.TryGetValue(type, out byte[] result) && result != null)
                    {
                        waitingAnswers.TryRemove(type, out byte[] t);

                        if (result.Length > 0)
                        {
                            switch (result[0] & 0b11000000)
                            {
                                case 0x40:
                                    throw new PacketException("Bad command");
                                case 0x80:
                                    throw new PacketException("Bad args");
                                case 0xC0:
                                    throw new PacketException("Device busy");
                            }
                        }

                        return result;
                    };
                }

                throw new TimeoutException($"No answer at {timeout} ms");
            });
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            var count = sp.BytesToRead;
            var data = new byte[count];
            var readed = sp.Read(data, 0, count);

            LogManager.WriteData($"{_serialPort.PortName} Receive: ", data);

            //push to queue
            for (int i = 0; i < readed; i++)
                receiveQueue.Enqueue(data[i]);

            //try to restore packet from queue
            ProcessQueue();
        }

        private void ProcessQueue()
        {
            while (receiveQueue.Count > 2)
            {
                //get marker
                var marker = receiveQueue.ElementAt(0);
                if (marker != 0xAB)
                {
                    receiveQueue.Dequeue();
                    continue;
                }

                //get length
                var length = receiveQueue.ElementAt(1);
                if (receiveQueue.Count < length+3)
                    return; //wait fully packet

                //process packet
                receiveQueue.Dequeue();
                receiveQueue.Dequeue();
                byte crc = GetAdditiveCrc(length, 0x8F);

                var data = new byte[length];
                for (int i = 0; i < length; i++)
                    data[i] = receiveQueue.Dequeue();

                var receivedcrc = receiveQueue.Dequeue();
                if (receivedcrc != GetCrc(data, 0, -1, crc))
                {
                    LogManager.WriteText("bad crc");
                    return;
                }

                ReceiveData(data);
            }
        }

        private void ReceiveData(byte[] data)
        {
            var messageType = GetMessageType(data);

            if (messageType == PacketType.Message)
                messageDelegate?.Invoke(System.Text.Encoding.ASCII.GetString(data, 1, data.Count() - 1));
            else if (messageType == PacketType.Error)
                errorDelegate?.Invoke((ErrorType)data[1], null);
            else if (messageType == PacketType.Event)
                eventDelegate?.Invoke((EventType)data[1], null);
            else {
                waitingAnswers.TryUpdate(messageType, data, null);
            }
        }

        private PacketType GetMessageType(byte[] data)
        {
            if (data.Length == 0)
                return PacketType.Ping;
            else if((PacketType)data[0] == PacketType.Message || (PacketType)data[0] == PacketType.Error || (PacketType)data[0] == PacketType.Event)
                return (PacketType)(data[0]);
            else
                return (PacketType)(data[0] & 0x3F);
        }

        private static byte GetAdditiveCrc(byte data, byte oldcrc)
        {
            for (byte bitCounter = 0; bitCounter < 8; bitCounter++)
            {
                if (((oldcrc ^ data) & 0x01) != 0)
                {
                    oldcrc = (byte)((oldcrc >> 1) ^ 0x8C);
                }
                else
                {
                    oldcrc >>= 1;
                }
                data >>= 1;
            }
            return oldcrc;
        }

        private static byte GetCrc(byte[] data, int offset = 0, int count = -1, byte startvalue = 0)
        {
            byte crc = startvalue;
            if (count < 0)
                count = data.Count();

            for (int index = offset; index < count; index++)
            {
                var currentByte = data[index];
                for (byte bitCounter = 0; bitCounter < 8; bitCounter++)
                {
                    if (((crc ^ currentByte) & 0x01) != 0)
                    {
                        crc = (byte)((crc >> 1) ^ 0x8C);
                    }
                    else
                    {
                        crc >>= 1;
                    }
                    currentByte >>= 1;
                }
            }
            return crc;
        }
    }
}
