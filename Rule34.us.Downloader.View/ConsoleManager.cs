﻿using Rule34.us.Downloader.Logic.Extensions;
using Rule34.us.Downloader.Logic.Utility;

namespace Rule34.us.Downloader.View
{
    internal static class ConsoleManager
    {
        private static readonly ConsoleColor titleColor = ConsoleColor.Red;
        private static readonly ConsoleColor descriptionColor = ConsoleColor.DarkYellow;
        private static readonly ConsoleColor tagsColor = ConsoleColor.Green;
        private const string title = "\r\n██████╗ ██╗   ██╗██╗     ███████╗██████╗ ██╗  ██╗   ██╗   ██╗███████╗\r\n██╔══██╗██║   ██║██║     ██╔════╝╚════██╗██║  ██║   ██║   ██║██╔════╝\r\n██████╔╝██║   ██║██║     █████╗   █████╔╝███████║   ██║   ██║███████╗\r\n██╔══██╗██║   ██║██║     ██╔══╝   ╚═══██╗╚════██║   ██║   ██║╚════██║\r\n██║  ██║╚██████╔╝███████╗███████╗██████╔╝     ██║██╗╚██████╔╝███████║\r\n╚═╝  ╚═╝ ╚═════╝ ╚══════╝╚══════╝╚═════╝      ╚═╝╚═╝ ╚═════╝ ╚══════╝\r\n";

        private const string author = "IBangedMyToaster";
        private const string version = "4.0.0";
        private const string site = @"https://rule34.us";
        private const string github = @"https://github.com/IBangedMyToaster/Rule34.us.Downloader";
        private const string description = $"Rule34 downloader allows you to download all the images present in {site} site\r\nJust enter the appropriate tags and it will download all images of that tag into your computer!\r\nFor tags follow the same convention that used in Rule34.\r\nFor more information visit the {github}";

        internal static void PrintTitle()
        {
            Console.Clear();

            Logger.LogSimple(title, titleColor);
            Logger.LogSimple(Write($"by {author}{new string(' ', 36)}Version: {version}"), titleColor);
            Logger.LogSimple(Write(description), descriptionColor);
            Logger.LogSimple(Write("Enter Tags: ", 0), tagsColor);
        }

        public static void EnterTags()
        {
            Logger.LogSimple(Write("Enter Tags: ", 0), tagsColor);
        }

        private static string Write(string msg, int spaces = 2)
        {
            return msg + new string('\n', spaces);
        }

        public static bool UserWantsToContinue()
        {
            Logger.LogSimple("\nEnter [c] to continue; else any other key to exit: ", ConsoleColor.DarkYellow);

            char input = Console.ReadKey().KeyChar;
            return input.Matches('c');
        }
    }
}
