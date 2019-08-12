using OpenWasherHardwareLibrary.Commands;
using OpenWasherHardwareLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static OpenWasherHardwareLibrary.HardwareLibrary;

namespace OpenWasherHardwareLibrary.Managers
{
    internal class CommandsManager : IDisposable
    {
        const int DELAY_AFTER_CONNECT = 3000;

        internal IOManager _io;

        private readonly MessageReceivedDelegate messageDelegate;
        private readonly ErrorReceivedDelegate errorDelegate;
        private readonly EventReceivedDelegate eventDelegate;
        private readonly ConnectionEventDelegate connectionEventDelegate;

        internal static IEnumerable<string> AvaliableComPorts => IOManager.GetAvaliableComPorts();

        internal CommandsManager(
            MessageReceivedDelegate messageDelegate = null,
            ErrorReceivedDelegate errorDelegate = null,
            EventReceivedDelegate eventDelegate = null,
            ConnectionEventDelegate connectionEventDelegate = null)
        {
            this.messageDelegate = messageDelegate;
            this.errorDelegate = errorDelegate;
            this.eventDelegate = eventDelegate;
            this.connectionEventDelegate = connectionEventDelegate;
        }

        internal async Task ConnectAsync(string port = null)
        {
            if (_io != null)
                _io.Dispose();

            if (!string.IsNullOrEmpty(port) && port != "AUTO")
            {
                //connect to port
                var result = await GetConnectionTask(port);
                if (result.Success)
                {
                    _io = result.ioManager;
                    connectionEventDelegate?.Invoke(ConnectionEventType.Connected, _io.Port);
                }
                else
                    connectionEventDelegate?.Invoke(ConnectionEventType.ConnectFailed, result.Error);
            }
            else
            {
                //try connect to all ports
                var ports = IOManager.GetAvaliableComPorts();
                var tasks = ports.Select(x => GetConnectionTask(x)).ToList();

                while (tasks.Count > 0)
                {
                    var connectTask = await Task.WhenAny(tasks).ConfigureAwait(false);
                    if (connectTask.Result.Success)
                    {
                        if (_io == null)
                        {
                            _io = connectTask.Result.ioManager;
                            connectionEventDelegate?.Invoke(ConnectionEventType.Connected, _io.Port);
                        }
                        else _io.Dispose();
                    }                  

                    tasks.Remove(connectTask);
                }

                if(_io == null)
                    connectionEventDelegate?.Invoke(ConnectionEventType.NotFound, null);
            }
        }

        private async Task<(bool Success, string Error, IOManager ioManager)> GetConnectionTask(string port)
        {
            IOManager ioManager = null;
            try
            {
                ioManager = new IOManager(port, messageDelegate, errorDelegate, eventDelegate);
                Thread.Sleep(DELAY_AFTER_CONNECT);
                await SendCommandAsync(ioManager, new Ping());
                return (true, null, ioManager);
            }
            catch( Exception e)
            {
                ioManager.Dispose();
                return (false, e.Message, null);
            }
        }

        internal void Disconnect()
        {
            if (_io != null)
                _io.Dispose();
        }

        public void Dispose()
        {
            if (_io != null)
                _io.Dispose();
        }

        internal async Task SendCommandAsync(IOManager iomanager, IWasherCommand command, int timeout = 10000, int tryCount = 3)
        {
            if (iomanager == null)
                throw new ApplicationException("No connection");

            do
            {
                try
                {
                    await iomanager.SendAsync(command.GetRequest(), timeout);
                    return;
                }
                catch (TimeoutException e)
                {
                    if (--tryCount <= 0)
                        throw e;
                }
            }
            while (tryCount > 0);

            throw new TimeoutException($"No answer at {timeout} ms");
        }

        internal async Task<TRESULT> SendCommandAsync<TRESULT>(IOManager iomanager, IWasherCommand<TRESULT> command, int timeout = 10000, int tryCount = 3)
        {
            if (iomanager == null)
                throw new ApplicationException("No connection");

            do
            {
                try
                {
                    var responce = await iomanager.SendAsync(command.GetRequest(), timeout);
                    return command.ParceResponse(responce);
                }
                catch(TimeoutException e)
                {
                    if (--tryCount <= 0)
                        throw e;
                }
            }
            while (tryCount > 0);

            throw new TimeoutException($"No answer at {timeout} ms");
        }
    }
}
