using Rule34.us.Downloader.Extensions;

namespace Rule34.us.Downloader.View
{
    internal static class ConsoleManager
    {
        private static ConsoleColor titleColor = ConsoleColor.Red;
        private static ConsoleColor descriptionColor = ConsoleColor.DarkYellow;
        private static ConsoleColor tagsColor = ConsoleColor.Green;
        private static ConsoleColor downloadColor = ConsoleColor.White;
        private const string title = " ________  ___  ___  ___       _______  ________  ___   ___      ___  ___  ________      \r\n|\\   __  \\|\\  \\|\\  \\|\\  \\     |\\  ___ \\|\\_____  \\|\\  \\ |\\  \\    |\\  \\|\\  \\|\\   ____\\     \r\n\\ \\  \\|\\  \\ \\  \\\\\\  \\ \\  \\    \\ \\   __/\\|____|\\ /\\ \\  \\\\_\\  \\   \\ \\  \\\\\\  \\ \\  \\___|_    \r\n \\ \\   _  _\\ \\  \\\\\\  \\ \\  \\    \\ \\  \\_|/__   \\|\\  \\ \\______  \\   \\ \\  \\\\\\  \\ \\_____  \\   \r\n  \\ \\  \\\\  \\\\ \\  \\\\\\  \\ \\  \\____\\ \\  \\_|\\ \\ __\\_\\  \\|_____|\\  \\ __\\ \\  \\\\\\  \\|____|\\  \\  \r\n   \\ \\__\\\\ _\\\\ \\_______\\ \\_______\\ \\_______\\\\_______\\     \\ \\__\\\\__\\ \\_______\\____\\_\\  \\ \r\n    \\|__|\\|__|\\|_______|\\|_______|\\|_______\\|_______|      \\|__\\|__|\\|_______|\\_________\\\r\n                                                                             \\|_________|";

        private const string author = "IBangedMyToaster";
        private const string site = @"https://rule34.us";
        private const string github = @"https://github.com/IBangedMyToaster/Rule34.us.Downloader";
        private const string description = $"Rule34 downloader allows you to download all the images present in {site} site\r\nJust enter the appropriate tags and it will download all images of that tag into your computer!\r\nFor tags follow the same convention that used in Rule34\r\nFor more information visit the {github}";

        internal static void PrintTitle()
        {
            Console.Clear();

            Logger.LogSimple(title, titleColor);
            Logger.LogSimpleAt(Write($"by {author}"), 5, Console.CursorTop, titleColor);
            Logger.LogSimple(Write(description), descriptionColor);
            Logger.LogSimple(Write("Enter Tags: ", 0), tagsColor);
        }

        public static void EnterTags()
        {
            Logger.LogSimple(Write("Enter Tags: ", 0), tagsColor);
        }

        private static string Write(string msg, int spaces = 2)
        {
            return msg + new String('\n', spaces);
        }

        public static bool UserWantsToContinue()
        {
            Logger.LogSimple(Write("Ready for another round!\nYou can download more images by continuing..", 2), descriptionColor);
            Logger.LogSimple("Enter [c] to continue; else any other key to exit: ");

            char input = Console.ReadKey().KeyChar;
            return input.Matches('c');
        }
    }
}
