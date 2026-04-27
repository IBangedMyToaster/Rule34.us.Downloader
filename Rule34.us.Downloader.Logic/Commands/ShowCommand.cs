using Rule34.us.Downloader.Logic.Rule34;
using Rule34.us.Downloader.Logic.Tagging;
using Rule34.us.Downloader.Logic.Utility;

namespace Rule34.us.Downloader.Logic.Commands
{
    public class ShowCommand : CommandBase
    {
        public ShowCommand(Tags tags, ConfigManager configManager) : base(tags, configManager)
        {
            Execute();
        }

        private void Execute()
        {
            Rule34Logistic logistic = new();

            // Get all ids by tags
            Logger.LogSimple($"Searching {string.Join(" ", this.Tags.Raw)}...\n", ConsoleColor.Yellow); // Log Checking
            List<Content> contentList = logistic.GetAllIdsByTags(this.Tags);

            Logger.LogSimple("Found", ConsoleColor.Yellow);
            Logger.LogSimple($" [{contentList.Count()}] ", ConsoleColor.White);
            LogUpdateProgress(contentList.Count()).Invoke();  // Log Result
        }

        private static Action LogUpdateProgress(int idCount)
        {
            return idCount switch
            {
                1 => () => Logger.LogSimple($"element with the given tags\n\n", ConsoleColor.Yellow),
                _ => () => Logger.LogSimple($"elements with the given tags\n\n", ConsoleColor.Yellow),
            };
        }
    }
}
