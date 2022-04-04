using System;
using System.Runtime.CompilerServices;
using System.IO;

namespace MinoTool
{
    public class LOG
    {
        /// <summary>Log a message to the console.</summary>
        public static void Log(object message, [CallerMemberName] string name = "", [CallerFilePath] string path = "", [CallerLineNumber] int line = 0)
        {
#if DEBUG
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"LOG:{Path.GetFileNameWithoutExtension(path)}: ({message}) line:{line}");
            Console.ForegroundColor = ConsoleColor.White;
#endif
        }

        public static void Error(object message, [CallerMemberName] string name = "", [CallerFilePath] string path = "", [CallerLineNumber] int line = 0)
        {
#if DEBUG
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"LOG:{Path.GetFileNameWithoutExtension(path)}: ({message}) line:{line}");
            Console.ForegroundColor = ConsoleColor.White;

#endif
        }
        public static void Warning(object message, [CallerMemberName] string name = "", [CallerFilePath] string path = "", [CallerLineNumber] int line = 0)
        {
#if DEBUG
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"LOG:{Path.GetFileNameWithoutExtension(path)}: ({message}) line:{line}");
            Console.ForegroundColor = ConsoleColor.White;
#endif
        }

        public static void Success(object message, [CallerMemberName] string name = "", [CallerFilePath] string path = "", [CallerLineNumber] int line = 0)
        {
#if DEBUG
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"LOG:{Path.GetFileNameWithoutExtension(path)}: ({message}) line:{line}");
            Console.ForegroundColor = ConsoleColor.White;
#endif
        }

        public static void LOG_PLAIN(object message)
        {
#if DEBUG
            Console.WriteLine(message);
#endif
        }
    }
}