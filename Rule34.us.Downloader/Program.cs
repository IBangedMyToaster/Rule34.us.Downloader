using Rule34.us.Downloader;
using System.Text;

namespace Rule34.us.Downloader.View
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Logger logger = new Logger(Encoding.UTF8);
            MainViewViewModel mainViewViewModel;
            ConsoleManager consoleManager = new ConsoleManager();
            List<string> tags;

            logger.DebugLog();

            do
            {

                consoleManager.PrintTitle();
                tags = new TagAnalyzer(Console.ReadLine()).Analyze();
                mainViewViewModel = new MainViewViewModel(tags, logger);
                // Download

            } while (consoleManager.UserWantsToContinue());
            

        }
    }
}