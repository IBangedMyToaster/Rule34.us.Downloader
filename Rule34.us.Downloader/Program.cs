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
            ConsoleManager consoleManager = new ConsoleManager(logger);
            List<string> tags;

            do
            {

                consoleManager.PrintTitle();
                tags = new TagAnalyzer(Console.ReadLine(), logger).Analyze();
                mainViewViewModel = new MainViewViewModel(tags, logger);
                // Download

            } while (consoleManager.UserWantsToContinue());
            

        }
    }
}