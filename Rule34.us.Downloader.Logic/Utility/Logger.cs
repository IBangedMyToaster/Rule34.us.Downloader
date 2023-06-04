namespace Rule34.us.Downloader.Logic.Utility
{
    public static class Logger
    {
        public static void CrashLog(string filename, string errorMessage)
        {
            if (!File.Exists(filename))
            {
                File.Create(filename).Close();
            }

            File.AppendAllLines(filename, new[] { $"[{DateTime.Now:HH:mm:ss | dd.MM.yyyy}] Error: {errorMessage}" });
        }

        //public static void PrintCommands(string[] commands)

        public static void LogSimpleAt(string message, int x, int y, ConsoleColor? color = null)
        {
            if (color != null)
            {
                Console.ForegroundColor = color.Value;
            }

            Console.SetCursorPosition(x, y);
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void LogSimple(string message, ConsoleColor? color = null)
        {
            if (color != null)
            {
                Console.ForegroundColor = color.Value;
            }

            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void LogOnSpot(string message, ConsoleColor? color = null, params object[]? args)
        {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;

            if (color != null)
            {
                Console.ForegroundColor = color.Value;
            }

            Console.Write(message, args);
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void LogMultipleOnSpot(string message1, string? message2 = null, string? message3 = null, int message2Offset = 0,
                                             ConsoleColor? color1 = null, ConsoleColor? color2 = null, ConsoleColor? color3 = null)
        {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;

            // Message 1
            Console.ForegroundColor = color1 ?? ConsoleColor.White;
            Console.Write(message1);

            Console.ForegroundColor = color2 ?? ConsoleColor.White;
            Console.Write(message2);

            Console.SetCursorPosition(message2Offset, y);

            // Message 3
            Console.ForegroundColor = color3 ?? ConsoleColor.White;
            Console.Write(message3);
            Console.SetCursorPosition(x, y);

            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void LogOnSpot(string message, ConsoleColor? color = null)
        {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;

            if (color != null)
            {
                Console.ForegroundColor = color.Value;
            }

            Console.Write(message);
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Log(string message, LogLevel level)
        {
            Tuple<string, ConsoleColor> tuple = GetColorByLogLevel(level);
            Console.ForegroundColor = tuple.Item2;

            if (level is LogLevel.Error or LogLevel.Debug)
            {
                File.AppendAllLines("log.txt", new List<string>() { tuple.Item1 + message });
            }

            Console.WriteLine(tuple.Item1 + message);

            Console.ForegroundColor = ConsoleColor.White;
        }

        private static Tuple<string, ConsoleColor> GetColorByLogLevel(LogLevel level)
        {
            return level switch
            {
                LogLevel.Error => new Tuple<string, ConsoleColor>("| ⛔ | ", ConsoleColor.Red),
                LogLevel.Warning => new Tuple<string, ConsoleColor>("| ⚠️ | ", ConsoleColor.DarkYellow),
                LogLevel.Information => new Tuple<string, ConsoleColor>("| ℹ️ | ", ConsoleColor.White),
                LogLevel.Debug => new Tuple<string, ConsoleColor>("| ♻️ | ", ConsoleColor.Magenta),
                LogLevel.Success => new Tuple<string, ConsoleColor>("| ✅ | ", ConsoleColor.Green),
                LogLevel.Waiting => new Tuple<string, ConsoleColor>("| 🔜 | ", ConsoleColor.DarkYellow),
                _ => throw new ArgumentOutOfRangeException(nameof(level)),
            };
        }
    }

    public enum LogLevel
    {
        Debug,
        Information,
        Warning,
        Success,
        Error,
        Waiting,
    }
}
