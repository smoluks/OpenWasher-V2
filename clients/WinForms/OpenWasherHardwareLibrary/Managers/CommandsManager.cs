using OpenWasherHardwareLibrary.Commands;
using OpenWasherHardwareLibrary.Enums;
using OpenWasherHardwareLibrary.Exceptions;
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

        private IOManager _io;

        private readonly MessageReceivedDelegate messageDelegate;
        private readonly ErrorReceivedDelegate errorDelegate;
        private readonly EventReceivedDelegate eventDelegate;

        internal static IEnumerable<string> AvaliableComPorts => IOManager.GetAvaliableComPorts();

        public bool IsConnected => _io != null;

        internal CommandsManager(
            MessageReceivedDelegate messageDelegate = null,
            ErrorReceivedDelegate errorDelegate = null,
            EventReceivedDelegate eventDelegate = null)
        {
            this.messageDelegate = messageDelegate;
            this.errorDelegate = errorDelegate;
            this.eventDelegate = eventDelegate;
        }

        internal async Task<(ConnectionEventType result, string port, string error)> ConnectAsync(CancellationToken token, string port = null)
        {
            if (_io != null)
                _io.Dispose();

            if (!string.IsNullOrEmpty(port) && port != "AUTO")
            {
                //connect to port
                try
                {
                    var (Success, Error, ioManager) = await GetConnectionTask(token, port);
                    token.ThrowIfCancellationRequested();

                    if (Success)
                    {
                        _io = ioManager;
                        return (ConnectionEventType.Connected, _io.Port, null);
                    }
                    else return (ConnectionEventType.ConnectFailed, null, Error);
                }
                catch(Exception e)
                {
                    return (ConnectionEventType.ConnectFailed, null, e.Message);
                }                
            }
            else
            {
                //try connect to all ports
                var ports = IOManager.GetAvaliableComPorts();
                var tasks = ports.Select(x => GetConnectionTask(token, x)).ToList();

                while (tasks.Count > 0)
                {
                    var connectTask = await Task.WhenAny(tasks).ConfigureAwait(false);
                    if (!connectTask.IsFaulted && connectTask.Result.Success)
                    {
                        if (_io == null)
                        {
                            _io = connectTask.Result.ioManager;
                            return (ConnectionEventType.Connected, _io.Port, null);
                        }
                        else _io.Dispose();
                    }

                    tasks.Remove(connectTask);
                }

                return (ConnectionEventType.NotFound, null, null);
            }
        }

        internal void Disconnect()
        {
            if (_io != null)
            {
                _io.Dispose();
                _io = null;
            }
        }

        public void Dispose()
        {
            if (_io != null)
                _io.Dispose();
        }

        private async Task<(bool Success, string Error, IOManager ioManager)> GetConnectionTask(CancellationToken token, string port)
        {
            IOManager ioManager = null;
            try
            {
                ioManager = new IOManager(port, messageDelegate, errorDelegate, eventDelegate);

                //wait washer bt start
                if (token.WaitHandle.WaitOne(DELAY_AFTER_CONNECT))
                {
                    throw new OperationCanceledException();
                }

                await SendCommandAsync(token, ioManager, new Ping());
                token.ThrowIfCancellationRequested();

                return (true, null, ioManager);
            }
            catch (Exception e)
            {
                ioManager.Dispose();
                return (false, e.Message, null);
            }
        }

        internal async Task SendCommandAsync(CancellationToken token, IWasherCommand command, int timeout)
        {
            await SendCommandAsync(token, _io, command, timeout);
        }

        private async Task SendCommandAsync(CancellationToken token, IOManager iomanager, IWasherCommand command, int timeout = 10000, int tryCount = 3)
        {
            if (iomanager == null)
                throw new CommandException("No connection");

            do
            {
                token.ThrowIfCancellationRequested();

                try
                {
                    await iomanager.SendAsync(token, command.GetRequest(), timeout);
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

        internal async Task<TRESULT> SendCommandAsync<TRESULT>(CancellationToken token, IWasherCommand<TRESULT> command, int timeout)
        {
            return await SendCommandAsync(token, _io, command, timeout);
        }
        private async Task<TRESULT> SendCommandAsync<TRESULT>(CancellationToken token, IOManager iomanager, IWasherCommand<TRESULT> command, int timeout = 10000, int tryCount = 3)
        {
            if (iomanager == null)
                throw new CommandException("No connection");

            do
            {
                token.ThrowIfCancellationRequested();
                try
                {
                    var responce = await iomanager.SendAsync(token, command.GetRequest(), timeout);
                    return command.ParceResponse(responce);
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
    }
}
