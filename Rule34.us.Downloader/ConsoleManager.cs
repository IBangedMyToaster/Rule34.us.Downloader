using Rule34.us.Downloader.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rule34.us.Downloader.View
{
    internal class ConsoleManager
    {
        private ConsoleColor titleColor = ConsoleColor.Red;
        private ConsoleColor descriptionColor = ConsoleColor.DarkYellow;
        private ConsoleColor tagsColor = ConsoleColor.Green;
        private ConsoleColor downloadColor = ConsoleColor.White;
        private Logger logger;
        private const string title = " ________  ___  ___  ___       _______  ________  ___   ___      ___  ___  ________      \r\n|\\   __  \\|\\  \\|\\  \\|\\  \\     |\\  ___ \\|\\_____  \\|\\  \\ |\\  \\    |\\  \\|\\  \\|\\   ____\\     \r\n\\ \\  \\|\\  \\ \\  \\\\\\  \\ \\  \\    \\ \\   __/\\|____|\\ /\\ \\  \\\\_\\  \\   \\ \\  \\\\\\  \\ \\  \\___|_    \r\n \\ \\   _  _\\ \\  \\\\\\  \\ \\  \\    \\ \\  \\_|/__   \\|\\  \\ \\______  \\   \\ \\  \\\\\\  \\ \\_____  \\   \r\n  \\ \\  \\\\  \\\\ \\  \\\\\\  \\ \\  \\____\\ \\  \\_|\\ \\ __\\_\\  \\|_____|\\  \\ __\\ \\  \\\\\\  \\|____|\\  \\  \r\n   \\ \\__\\\\ _\\\\ \\_______\\ \\_______\\ \\_______\\\\_______\\     \\ \\__\\\\__\\ \\_______\\____\\_\\  \\ \r\n    \\|__|\\|__|\\|_______|\\|_______|\\|_______\\|_______|      \\|__\\|__|\\|_______|\\_________\\\r\n                                                                             \\|_________|";

        private const string author = "IBangedMyToaster";
        private const string site = @"https://rule34.us";
        private const string github = @"https://github.com/PlaceHolder";
        private const string description = $"Rule34 downloader allows you to download all the images present in {site} site\r\nJust enter the appropriate tags and it will download all images of that tag into your computer!\r\nFor tags follow the same convention that used in Rule34\r\nFor more information visit the {github}";

        public ConsoleManager(Logger logger)
        {
            this.logger = logger;
        }

        internal void PrintTitle()
        {
            Console.Clear();

            logger.LogSimple(title, titleColor);
            logger.LogSimpleAt(Write($"by {author}"), 5, Console.CursorTop, titleColor);
            logger.LogSimple(Write(description), descriptionColor);
            logger.LogSimple(Write("Enter Tags: ", 0), tagsColor);
        }

        private string Write(string msg, int spaces = 2)
        {
            return msg + new String('\n', spaces);
        }

        internal bool UserWantsToContinue()
        {
            logger.LogSimple(Write("Do you want to continue? (Y/N): ", 0), titleColor);

            do
            {
                string input = Console.ReadLine();

                if (input.Matches("n"))
                    return false;
                else if (input.Matches("y"))
                    return true;

                Console.ForegroundColor = ConsoleColor.Red;
                ;
                Console.ForegroundColor = ConsoleColor.White;

                logger.LogSimple(Write($"The Input \"{input}\" is invalid!", 1), ConsoleColor.Red);

            } while (true);
        }
    }
}
