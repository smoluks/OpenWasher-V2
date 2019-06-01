using OpenWasherHardwareLibrary.Entity;
using OpenWasherHardwareLibrary.Enums;
using OpenWasherHardwareLibrary.Managers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenWasherHardwareLibrary
{
    public class HardwareLibrary : IDisposable
    {
        #region Delegates

        public delegate void MessageReceivedDelegate(string message);
        public delegate void ErrorReceivedDelegate(ErrorType type, byte[] data);
        public delegate void EventReceivedDelegate(EventType type, byte[] data);

        #endregion

        #region Fields

        private CommandsManager _commgr;

        #endregion

        #region Properties

        public bool LoggingEnable
        {
            get => LogManager.Enable;
            set => LogManager.Enable = value;
        }

        #endregion

        #region Constructor

        public HardwareLibrary(
            MessageReceivedDelegate messageDelegate = null,
            ErrorReceivedDelegate errorDelegate = null,
            EventReceivedDelegate eventDelegate = null)
        {
            _commgr = new CommandsManager(messageDelegate, errorDelegate, eventDelegate);
        }

        #endregion

        #region Public methods

        public async Task ConnectAsync(string port)
        {
            await _commgr.ConnectAsync(port);
        }

        public async Task<string> ConnectAsync()
        {
            return await _commgr.ConnectAsync();
        }

        public void Disconnect()
        {
            _commgr.Disconnect();
        }

        public void Dispose()
        {
            _commgr.Dispose();
        }        

        public async Task StartProgramAsync(Programs program, ProgramOptions options = null)
        {
            await _commgr.StartProgramAsync(program, options);
        }

        public async Task StopProgramAsync()
        {
            await _commgr.StopProgramAsync();
        }

        public async Task GoToBootloaderAsync()
        {
            await _commgr.GoToBootloaderAsync();
        }

        public async Task<Status> GetStatusAsync()
        {
            return await _commgr.GetStatusAsync();
        }

        public static IEnumerable<string> GetComPorts()
        {
            return CommandsManager.AvaliableComPorts;
        }

        #endregion
    }
}
