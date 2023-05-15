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
            string[] ids = logistic.GetAllIdsByTags(Tags);

            Logger.LogSimple("Found", ConsoleColor.Yellow);
            Logger.LogSimple($" [{ids.Length}] ", ConsoleColor.White);
            LogUpdateProgress(ids.Length).Invoke();  // Log Result
        }

        private static Action LogUpdateProgress(int idCount)
        {
            return idCount switch
            {
                1 => () => Logger.LogSimple($"element with the given Tags\n\n", ConsoleColor.Yellow),
                _ => () => Logger.LogSimple($"elements with the given Tags\n\n", ConsoleColor.Yellow),
            };
        }
    }
}
