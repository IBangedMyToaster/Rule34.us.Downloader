using Rule34.us.Downloader.Logic.Rule34;
using Rule34.us.Downloader.Logic.Tags;
using Rule34.us.Downloader.Logic.Utility;

namespace Rule34.us.Downloader.Logic.Commands
{
    public class UpdateCommand
    {
        public Tags.Tags Tags { get; private set; }
        public ConfigManager ConfigManager { get; }

        private readonly Rule34Logistic logistic = new();

        public UpdateCommand(Tags.Tags tags, ConfigManager configManager)
        {
            Tags = tags ?? throw new ArgumentNullException(nameof(tags));
            ConfigManager = configManager;

            if (Tags.TrimmedInput().Any())
            {
                UpdateSpecific(TagDirectory.GetDirectoryByTags(ConfigManager.Configuration, Tags));
                return;
            }

            UpdateAll();
        }

        private void UpdateSpecific(TagDirectory tagDirectory)
        {
            // Get all ids by tags
            Logger.LogSimple($"Updating {tagDirectory.Name}...\n", ConsoleColor.Yellow); // Log Checking
            string[] ids = logistic.GetAllIdsByTagsTill(tagDirectory.GetLastId(), tagDirectory.Tags, out int pages);

            // Get all links by ids
            Dictionary<string, string> idLinkPairs = logistic.ConvertIdsToLinks(ids);

            // Dowload Files by links
            _ = logistic.Download(tagDirectory.OriginalPath, idLinkPairs);
            LogUpdateProgress(ids.Count(), tagDirectory).Invoke();  // Log Result
        }

        private void UpdateAll()
        {
            TagDirectory[] tagDirectories = Directory.GetDirectories(ConfigManager.GetSavePath).Select(dir => new TagDirectory(dir)).ToArray();

            foreach (TagDirectory tagDirectory in tagDirectories)
            {
                UpdateSpecific(tagDirectory);
            }
        }

        private Action LogUpdateProgress(int idCount, TagDirectory directory)
        {
            return idCount switch
            {
                0 => () => Logger.LogSimple($"\"{directory.Name}\" - Up-To-Date\n\n", ConsoleColor.Green),
                1 => () => Logger.LogSimple($"\"{directory.Name}\" - Added {idCount} element\n\n", ConsoleColor.Yellow),
                _ => () => Logger.LogSimple($"\"{directory.Name}\" - Added {idCount} elements\n\n", ConsoleColor.Yellow),
            };
        }
    }
}
