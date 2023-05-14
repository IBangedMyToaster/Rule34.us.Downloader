using Rule34.us.Downloader.Logic.Rule34;
using Rule34.us.Downloader.Logic.Utility;

namespace Rule34.us.Downloader.Logic.Commands
{
    public class ShowCommand
    {
        public Tags.Tags Tags { get; private set; }

        public ShowCommand(Tags.Tags tags)
        {
            Tags = tags ?? throw new ArgumentNullException(nameof(tags));
            Execute();
        }

        private void Execute()
        {
            Rule34Logistic logistic = new();

            // Get all ids by tags
            Logger.LogSimple($"Searching {string.Join(" ", Tags.Raw)}...\n", ConsoleColor.Yellow); // Log Checking
            string[] ids = logistic.GetAllIdsByTags(Tags, out int pages);
            LogUpdateProgress(ids.Count()).Invoke();  // Log Result
        }

        private Action LogUpdateProgress(int idCount)
        {
            return idCount switch
            {
                0 => () => Logger.LogSimple($"Nothing was found with the given Tags\n\n", ConsoleColor.Red),
                1 => () => Logger.LogSimple($"Found {idCount} element with the given Tags\n\n", ConsoleColor.Yellow),
                _ => () => Logger.LogSimple($"Found {idCount} elements with the given Tags\n\n", ConsoleColor.Yellow),
            };
        }
    }
}
