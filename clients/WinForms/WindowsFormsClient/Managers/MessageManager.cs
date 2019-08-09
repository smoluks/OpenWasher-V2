using System;
using System.Collections.Generic;
using WindowsFormsClient.Entities;

namespace WindowsFormsClient.Managers
{
    internal static class MessageManager
    {
        public delegate void NewLogMessageDelegate(Message log);

        public static event NewLogMessageDelegate NewLogMessageEvent;

        internal static void MessageHandler(string message)
        {
            throw new NotImplementedException();
        }

        internal static IEnumerable<Message> GetMessages()
        {
            throw new NotImplementedException();
        }
    }
}
