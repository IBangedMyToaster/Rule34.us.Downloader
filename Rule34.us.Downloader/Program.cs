using System.Text;

namespace Rule34.us.Downloader.View
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Logger logger = new Logger(Encoding.UTF8);
            MainViewViewModel mainViewViewModel;
            ConsoleManager consoleManager = new ConsoleManager(logger);
            List<string> tags;

            try
            {
                do
                {

                    consoleManager.PrintTitle();
                    tags = new TagAnalyzer(Console.ReadLine(), logger).Analyze();

                    if (tags.Any() && IsACommand(tags))
                        mainViewViewModel = new MainViewViewModel(tags.First(), tags.Skip(1).ToList(), logger);
                    else
                        mainViewViewModel = new MainViewViewModel(tags, logger);
                
                    // Download

                } while (consoleManager.UserWantsToContinue());
            }
            catch (Exception e)
            {
                logger.CrashLog("creashReport.txt", e.Message);
            }
        }

        private static bool IsACommand(List<string> tags)
        {
            return tags.First().StartsWith("--");
        }
    }
}