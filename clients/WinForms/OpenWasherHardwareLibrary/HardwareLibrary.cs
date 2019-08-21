using OpenWasherHardwareLibrary.Commands;
using OpenWasherHardwareLibrary.Enums;
using OpenWasherHardwareLibrary.Entity;
using OpenWasherHardwareLibrary.Managers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenWasherHardwareLibrary
{
    public class HardwareLibrary : IDisposable
    {
        #region Delegates

        //callback for message from washing machine
        public delegate void MessageReceivedDelegate(string message);

        //callback for errros from washing machine
        public delegate void ErrorReceivedDelegate(ErrorType type, byte[] data);

        public void updateFirmware(byte[] firmware)
        {
            throw new NotImplementedException();
        }

        //callback for events from washing machine
        public delegate void EventReceivedDelegate(EventType type, byte[] data);

        //callback for com port connection events
        public delegate void ConnectionEventDelegate(ConnectionEventType type, string text);

        #endregion

        #region Fields

        private readonly CommandsManager _commgr;

        public bool LogEnable { get; set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Making connect to washer
        /// </summary>
        /// <param name="port">COM port from GetComPorts() list. Null = scan all</param>
        /// <param name="messageDelegate">Callback to process washer message</param>
        /// <param name="errorDelegate">Callback to process washer message</param>
        /// <param name="eventDelegate">Callback to process washer message</param>
        /// <param name="connectionEventDelegate">Callback to process washer message</param>
        /// <param name="logEnable">Enable log?</param>
        public HardwareLibrary(
            string port = null,
            MessageReceivedDelegate messageDelegate = null,
            ErrorReceivedDelegate errorDelegate = null,
            EventReceivedDelegate eventDelegate = null,
            ConnectionEventDelegate connectionEventDelegate = null,
            bool logEnable = false)
        {
            _commgr = new CommandsManager(messageDelegate, errorDelegate, eventDelegate, connectionEventDelegate);
            LogManager.Enable = logEnable;
            Task.Run(() => _commgr.ConnectAsync(port));
        }

        #endregion

        #region Public methods

        public void Dispose()
        {
            _commgr.Dispose();
        }

        /// <summary>
        /// Send command
        /// </summary>
        public async Task SendCommandAsync(IWasherCommand command, int timeout = 1000)
        {
            await _commgr.SendCommandAsync(command, timeout);
        }

        /// <summary>
        /// Send command with receive answers
        /// </summary>
        public async Task<TRESULT> SendCommandAsync<TRESULT>(IWasherCommand<TRESULT> command, int timeout = 1000)
        {
            return await _commgr.SendCommandAsync(command, timeout);
        }

        /// <summary>
        /// Get default program options
        /// TODO: get remote
        /// </summary>
        /// <returns></returns>
        public static ProgramOptions GetDefaultOptions(WashProgram program)
        {
            return ParamsManager.GetDefault(program);
        }

        /// <summary>
        /// Get all avaliables COM-ports from system
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetComPorts()
        {
            return CommandsManager.AvaliableComPorts;
        }

        #endregion
    }
}
