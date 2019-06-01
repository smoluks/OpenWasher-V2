using System;
using System.IO;

namespace OpenWasherHardwareLibrary.Managers
{
    internal class LogManager
    {
        private static object locker = new object();

        internal static bool Enable { get; set; } = true;

        internal static void WriteText(string message)
        {
            if (!Enable)
                return;

            Write(message);
        }

        internal static void WriteData(string message, byte[] data, int startIndex = 0, int length = -1)
        {
            if (!Enable)
                return; 

            if (length == -1)
                length = data.Length;

            Write(message + '\n' + BitConverter.ToString(data, startIndex, length).Replace("-", " "));
        }

        internal static void WriteException(string message, Exception e)
        {
            if (!Enable)
                return;

            Write($"{message}'\n'{e.Message}'\n'{e.StackTrace}");
            if(e.InnerException != null)
                Write($"InnerException\n{message}'\n'{e.Message}'\n'{e.StackTrace}");
        }

        private static void Write(string text)
        {
            lock (locker)
            {
                if (!Directory.Exists("logs"))
                    Directory.CreateDirectory("logs");
                File.AppendAllText($"logs\\{DateTime.Now.ToString("dd_MM_yyyy")}.txt", $"{DateTime.Now.ToString("G")} {text}\n");
            }
        }
    }
}
