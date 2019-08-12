using System.Collections.Generic;
using WindowsFormsClient.Entities;

namespace WindowsFormsClient.Managers
{
    internal static class MessageManager
    {
        public delegate void NewLogMessageDelegate(Message log);
        public static event NewLogMessageDelegate NewLogMessageEvent;

        private static readonly List<Message> messages = new List<Message>();

        internal static void MessageHandler(string message)
        {
            var msg = new Message(message);
            messages.Add(msg);
            NewLogMessageEvent?.Invoke(msg);
        }

        internal static IEnumerable<Message> GetMessages()
        {
            return messages;
        }
    }
}
