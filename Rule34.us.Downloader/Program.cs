using System.Text;

namespace Rule34.us.Downloader.View
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MainViewViewModel mainViewViewModel;
            List<string> tags;

            try
            {
                do
                {
                    ConsoleManager.PrintTitle();
                    tags = new TagAnalyzer(Console.ReadLine()).Analyze();

                    if (tags.Any() && IsACommand(tags))
                        mainViewViewModel = new MainViewViewModel(tags.First(), tags.Skip(1).ToList());
                    else
                        mainViewViewModel = new MainViewViewModel(tags);
                
                    // Download

                } while (ConsoleManager.UserWantsToContinue());
            }
            catch (Exception e)
            {
                Logger.CrashLog("creashReport.txt", e.Message);
            }
        }

        private static bool IsACommand(List<string> tags)
        {
            return tags.First().StartsWith("--");
        }
    }
}