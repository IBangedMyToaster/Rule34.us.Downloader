using Rule34.us.Downloader.Logic.Rule34;
using Rule34.us.Downloader.Logic.Tags;
using Rule34.us.Downloader.Logic.Utility;

namespace Rule34.us.Downloader.Logic.Commands
{
    public class CompleteCommand
    {
        public Tags.Tags Tags { get; private set; }
        public ConfigManager ConfigManager { get; }

        private readonly Rule34Logistic logistic = new();

        public CompleteCommand(Tags.Tags tags, ConfigManager configManager)
        {
            Tags = tags ?? throw new ArgumentNullException(nameof(tags));
            ConfigManager = configManager;

            if (Tags.TrimmedInput().Any())
            {
                CompleteSpecific(TagDirectory.GetDirectoryByTags(ConfigManager.Configuration, Tags));
                return;
            }

            CompleteAll();
        }

        private void CompleteSpecific(TagDirectory tagDirectory)
        {
            // Get all ids by tags
            Logger.LogSimple($"Completing {string.Join(" ", tagDirectory.Tags.TrimmedInput())}...\n", ConsoleColor.Yellow); // Log Checking
            string[] ids = logistic.GetAllIdsByTags(tagDirectory.Tags, out int pages);

            // Compare existing Folder with Links and Filter doubles
            ids = ids.Except(tagDirectory.GetFilenames()).ToArray();

            // Get all links by ids
            Dictionary<string, string> idLinkPairs = logistic.ConvertIdsToLinks(ids);

            // Download all files by links and save in folder
            _ = logistic.Download(tagDirectory.OriginalPath, idLinkPairs);

            LogUpdateProgress(ids.Count(), tagDirectory).Invoke();  // Log Result
        }

        private void CompleteAll()
        {
            TagDirectory[] tagDirectories = Directory.GetDirectories(ConfigManager.GetSavePath).Select(dir => TagDirectory.GetDirectoryByTags(ConfigManager.Configuration, TagDirectory.GetTagsByPath(dir))).ToArray();

            foreach (TagDirectory tagDirectory in tagDirectories)
            {
                CompleteSpecific(tagDirectory);
            }
        }

        private Action LogUpdateProgress(int idCount, TagDirectory directory)
        {
            return idCount switch
            {
                0 => () => Logger.LogSimple($"\"{directory.Name}\" - Up-To-Date\n\n", ConsoleColor.Green),
                1 => () => Logger.LogSimple($"Completed \"{directory.Name}\" with {idCount} new element\n\n", ConsoleColor.Yellow),
                _ => () => Logger.LogSimple($"Completed \"{directory.Name}\" with {idCount} new elements\n\n", ConsoleColor.Yellow),
            };
        }
    }
}
