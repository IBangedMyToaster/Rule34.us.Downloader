using System.Text;

namespace Rule34.us.Downloader
{
    public static class Logger
    {
        public static void CrashLog(string filename, string errorMessage)
        {
            if(!File.Exists(filename))
                File.Create(filename).Close();

            File.AppendAllLines(filename, new[] { $"[{DateTime.Now.ToString("HH:mm:ss | dd.MM.yyyy")}] Error: {errorMessage}" });
        }

        //public static void PrintCommands(string[] commands)

        public static void LogSimpleAt(string message, int x, int y, ConsoleColor? color = null)
        {
            if (color != null)
                Console.ForegroundColor = color.Value;

            Console.SetCursorPosition(x, y);
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void LogSimple(string message, ConsoleColor? color = null)
        {
            if (color != null)
                Console.ForegroundColor = color.Value;

            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Log(string message, LogLevel level)
        {
            var tuple = GetColorByLogLevel(level);
            Console.ForegroundColor = tuple.Item2;

            if (level == LogLevel.Error || level == LogLevel.Debug)
                File.AppendAllLines("log.txt", new List<string>() { tuple.Item1 + message });

            Console.WriteLine(tuple.Item1 + message);

            Console.ForegroundColor = ConsoleColor.White;
        }

        private static Tuple<string, ConsoleColor> GetColorByLogLevel(LogLevel level) => level switch
        {
            LogLevel.Error => new Tuple<string, ConsoleColor>("| ⛔ | ", ConsoleColor.Red),
            LogLevel.Warning => new Tuple<string, ConsoleColor>("| ⚠️ | ", ConsoleColor.DarkYellow),
            LogLevel.Information => new Tuple<string, ConsoleColor>("| ℹ️ | ", ConsoleColor.White),
            LogLevel.Debug => new Tuple<string, ConsoleColor>("| ♻️ | ", ConsoleColor.Magenta),
            LogLevel.Success => new Tuple<string, ConsoleColor>("| ✅ | ", ConsoleColor.Green),
            LogLevel.Waiting => new Tuple<string, ConsoleColor>("| 🔜 | ", ConsoleColor.Gray),
            _ => throw new ArgumentOutOfRangeException(nameof(level)),
        };
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
