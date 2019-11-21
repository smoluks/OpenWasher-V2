using OpenWasherHardwareLibrary.Commands;
using OpenWasherHardwareLibrary.Enums;
using OpenWasherHardwareLibrary.Entity;
using OpenWasherHardwareLibrary.Managers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace OpenWasherHardwareLibrary
{
    public class HardwareLibrary : IDisposable
    {
        const int DEFAULT_TIMEOUT = 3000;

        #region Delegates

        //callback for message from washing machine
        public delegate void MessageReceivedDelegate(string message);

        //callback for errros from washing machine
        public delegate void ErrorReceivedDelegate(ErrorType type, byte[] data);

        //callback for events from washing machine
        public delegate void EventReceivedDelegate(EventType type, byte[] data);

        #endregion

        #region Fields

        private readonly CommandsManager _commandsManager;

        /// <summary>
        /// Write binary log
        /// </summary>
        public bool LogEnable { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Get all avaliables COM-ports from system
        /// </summary>
        public static IEnumerable<string> GetComPorts
        {
            get => CommandsManager.AvaliableComPorts;
        }

        public bool IsConnected
        {
            get => _commandsManager.IsConnected;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Making connect to washer
        /// </summary>
        /// <param name="port">COM port from GetComPorts() list. Null = scan all</param>
        /// <param name="messageDelegate">Callback to process washer message</param>
        /// <param name="errorDelegate">Callback to process washer message</param>
        /// <param name="eventDelegate">Callback to process washer message</param>
        /// <param name="logEnable">Enable log?</param>
        public HardwareLibrary(
            MessageReceivedDelegate messageDelegate = null,
            ErrorReceivedDelegate errorDelegate = null,
            EventReceivedDelegate eventDelegate = null,
            bool logEnable = false)
        {
            _commandsManager = new CommandsManager(messageDelegate, errorDelegate, eventDelegate);
            LogManager.Enable = logEnable;
        }

        #endregion

        #region Public methods

        public async Task<(ConnectionEventType result, string port, string error)> ConnectAsync(CancellationToken token, string port = null)
        {
            return await _commandsManager.ConnectAsync(token, port);
        }

        public void Disconnect()
        {
            _commandsManager.Disconnect();
        }

        public void Dispose()
        {
            _commandsManager.Dispose();
        }

        /// <summary>
        /// Ping
        /// </summary>
        public async Task PingAsync(CancellationToken token)
        {

            await _commandsManager.SendCommandAsync(token, new Ping(), DEFAULT_TIMEOUT);
        }

        /// <summary>
        /// Start washing program
        /// </summary>
        public async Task StartProgramAsync(CancellationToken token, WashProgram program, ProgramOptions options = null, DateTime? startDate = null)
        {

            await _commandsManager.SendCommandAsync(token, new StartProgram(program, options, startDate), DEFAULT_TIMEOUT);
        }

        /// <summary>
        /// Stop washing program
        /// </summary>
        public async Task StopProgramAsync(CancellationToken token)
        {
            await _commandsManager.SendCommandAsync(token, new StopProgram(), DEFAULT_TIMEOUT);
        }

        /// <summary>
        /// Get default program options
        /// </summary>
        public async Task<ProgramOptions> GetDefaultOptionsAsync(CancellationToken token, WashProgram program)
        {
            return await _commandsManager.SendCommandAsync(token, new GetDefaultOptions(program), DEFAULT_TIMEOUT);
        }

        /// <summary>
        /// Switch to bootloader
        /// </summary>
        public async Task SwitchToBootloaderAsync(CancellationToken token)
        {
            await _commandsManager.SendCommandAsync(token, new GoToBootloader(), DEFAULT_TIMEOUT);
        }

        /// <summary>
        /// Get status
        /// </summary>
        public async Task<Status> GetStatusAsync(CancellationToken token)
        {
            return await _commandsManager.SendCommandAsync(token, new GetStatus(), DEFAULT_TIMEOUT);
        }

        /// <summary>
        /// Read washer configuration EEPROM
        /// </summary>
        public async Task<byte[]> GetEEPROM(CancellationToken token)
        {
            return await _commandsManager.SendCommandAsync(token, new GetConfig(), DEFAULT_TIMEOUT);
        }

        /// <summary>
        /// Set washer configuration config (may destroy washer)
        /// </summary>
        public async Task SetEEPROM(CancellationToken token, byte[] eeprom)
        {
            await _commandsManager.SendCommandAsync(token, new SetConfig(eeprom), DEFAULT_TIMEOUT);
        }

        /// <summary>
        /// Read washer time/PPM correction
        /// </summary>
        public async Task<(DateTime time, sbyte correction)> GetTime(CancellationToken token)
        {
            return await _commandsManager.SendCommandAsync(token, new GetTime(), DEFAULT_TIMEOUT);
        }

        /// <summary>
        /// Set washer time/PPM correction
        /// </summary>
        public async Task SetTime(CancellationToken token, DateTime? time = null, sbyte? correction = null)
        {
            await _commandsManager.SendCommandAsync(token, new SetTime(time, correction), DEFAULT_TIMEOUT);
        }

        #endregion
    }
}
