using System;

namespace WindowsFormsClient.Entities
{
    public class Message
    {
        public Message(string message)
        {
            this.message = message;
            this.timestamp = DateTime.Now;
        }

        public string message;
        public DateTime timestamp;

        public override string ToString()
        {
            return timestamp.ToString("T") + " " + message;
        }
    }
}
