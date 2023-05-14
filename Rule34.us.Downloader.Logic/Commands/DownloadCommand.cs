using Rule34.us.Downloader.Logic.Rule34;
using Rule34.us.Downloader.Logic.Tags;
using Rule34.us.Downloader.Logic.Utility;

namespace Rule34.us.Downloader.Logic.Commands
{
    public class DownloadCommand
    {
        public Tags.Tags Tags { get; private set; }
        public ConfigManager ConfigManager { get; }

        public DownloadCommand(Tags.Tags tags, ConfigManager configManager)
        {
            Tags = tags ?? throw new ArgumentNullException(nameof(tags));
            ConfigManager = configManager;
            Execute();
        }

        private void Execute()
        {
            Rule34Logistic logistic = new();

            // Get all ids by tags
            Logger.LogSimple($"Downloading {string.Join(" ", Tags.Raw)}...\n", ConsoleColor.Yellow); // Log Checking
            string[] ids = logistic.GetAllIdsByTags(Tags, out int pages);
            TagDirectory? tagDirectory = null;

            if (ids.Any())
            {
                // Get all links by ids
                Dictionary<string, string> idLinkPairs = logistic.ConvertIdsToLinks(ids);

                // Download all files by links and save in folder
                tagDirectory = TagDirectory.GetDirectoryByTags(ConfigManager.Configuration, Tags);
                _ = logistic.Download(tagDirectory.OriginalPath, idLinkPairs);
            }

            LogUpdateProgress(ids.Count(), tagDirectory).Invoke();  // Log Result
        }

        private Action LogUpdateProgress(int idCount, TagDirectory? tagDirectory)
        {
            return idCount switch
            {
                0 => () => Logger.LogSimple($"No Elements with were found with the given Tags!\n\n", ConsoleColor.Red),
                _ => () => Logger.LogSimple($"Saved {idCount} Elements in \"{tagDirectory.Name}\".\n\n", ConsoleColor.Yellow),
            };
        }
    }
}
