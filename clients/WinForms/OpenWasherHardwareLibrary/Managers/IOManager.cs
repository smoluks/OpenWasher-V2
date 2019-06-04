using OpenWasherHardwareLibrary.Enums;
using OpenWasherHardwareLibrary.Exceptions;
using OpenWasherHardwareLibrary.Managers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using static OpenWasherHardwareLibrary.HardwareLibrary;

namespace OpenWasherHardwareLibrary
{
    internal class IOManager : IDisposable
    {
        const int baudrate = 115200;
        const Parity parity = Parity.Even;
        const int timeout = 5000;
        const int readTimeGap = 200;

        private readonly MessageReceivedDelegate messageDelegate;
        private readonly ErrorReceivedDelegate errorDelegate;
        private readonly EventReceivedDelegate eventDelegate;

        internal string Port { get; private set; }

        SerialPort _serialPort;
        readonly Queue<byte> receiveQueue = new Queue<byte>();
        private ConcurrentDictionary<AnswerType, byte[]> waitingAnswers = new ConcurrentDictionary<AnswerType, byte[]>();

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
                ReadTimeout = timeout,
                WriteTimeout = timeout
            };

            try
            {
                _serialPort.Open();
                _serialPort.DataReceived += DataReceivedHandler;
            }
            catch (Exception e)
            {
                LogManager.WriteException($"{_serialPort.PortName} Open exception:", e);
                throw;
            }
        }

        public void Dispose()
        {
            if (_serialPort == null)
                return;

            if(_serialPort.IsOpen)
                _serialPort.Close();

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

        internal async Task<byte[]> SendAsync(byte[] buffer, int timeout)
        {
            var array = new byte[buffer.Count() + 3];
            array[0] = 0xAB;
            array[1] = (byte)buffer.Count();
            buffer.CopyTo(array, 2);
            array[buffer.Count() + 2] = GetCrc(array, 0, buffer.Count() + 2);

            LogManager.WriteData($"{_serialPort.PortName} Send: ", array);

            try
            {
                _serialPort.Write(array, 0, buffer.Count() + 3);
            }
            catch(Exception e)
            {
                LogManager.WriteException($"{_serialPort.PortName} Send exception:", e);
                throw;
            }

            var type = GetMessageType(buffer);
            waitingAnswers.AddOrUpdate(type, (key) => null, (key, oldValue) => null);

            timeout /= readTimeGap;
            while (timeout-- != 0)
            {
                if (waitingAnswers.TryGetValue(type, out byte[] result) && result != null)
                {
                    waitingAnswers.TryRemove(type, out byte[] t);

                    if(result.Length > 0)
                    {
                        switch(result[0] & 0b11000000)
                        {
                            case 0x40:
                                throw new CommandException("Bad command");
                            case 0x80:
                                throw new CommandException("Bad args");
                            case 0xC0:
                                throw new CommandException("Device busy");
                        }
                    }

                    return result;
                };

                await Task.Delay(readTimeGap);
            }

            throw new TimeoutException($"No answer at {timeout} ms");
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            var count = sp.BytesToRead;
            var data = new byte[count];
            var readed = sp.Read(data, 0, count);

            LogManager.WriteData($"{_serialPort.PortName} Receive: ", data);

            for (int i = 0; i < readed; i++)
                receiveQueue.Enqueue(data[i]);

            ProcessQueue();
        }

        private void ProcessQueue()
        {
            while (receiveQueue.Count > 2)
            {
                //get marker
                var marker = receiveQueue.Peek();
                if (marker != 0xAB)
                {
                    receiveQueue.Dequeue();
                    continue;
                }

                //get length
                var length = receiveQueue.ElementAt(1);
                if (receiveQueue.Count < length+3)
                    return;

                //process
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

            if (messageType == AnswerType.Message)
                messageDelegate?.Invoke(System.Text.Encoding.ASCII.GetString(data, 1, data.Count() - 1));
            else if (messageType == AnswerType.Error)
                errorDelegate?.Invoke((ErrorType)data[1], null);
            else if (messageType == AnswerType.Event)
                eventDelegate?.Invoke((EventType)data[1], null);
            else {
                waitingAnswers.TryUpdate(messageType, data, null);
            }
        }

        private AnswerType GetMessageType(byte[] data)
        {
            if (data.Length == 0)
                return AnswerType.Ping;
            else
                return (AnswerType)(data[0]);
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
