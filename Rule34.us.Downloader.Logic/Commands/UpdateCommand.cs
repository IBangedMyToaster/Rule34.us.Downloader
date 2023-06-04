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
                if (!TagDirectory.Exists(ConfigManager.Configuration, Tags))
                {
                    Logger.LogSimple($"The Folder \"{(string.Join(" ", Tags.TrimmedInput()))}\" does not Exist!\n", ConsoleColor.Red);
                    return;
                }

                UpdateSpecific(TagDirectory.GetTagDirectoryByTags(ConfigManager.Configuration, Tags));
                return;
            }

            UpdateAll();
        }

        private void UpdateSpecific(TagDirectory tagDirectory)
        {
            // Get all ids by tags
            Logger.LogSimple($"Updating {tagDirectory.Name}...\n", ConsoleColor.Yellow); // Log Checking
            List<Content> contentList = logistic.GetAllIdsByTags(tagDirectory.Tags, tagDirectory.GetLastId());

            // Get all links by ids
            logistic.GetLinks(contentList);

            // Dowload Files by links
            logistic.Download(tagDirectory.OriginalPath, contentList);
            LogUpdateProgress(contentList.Count(), tagDirectory).Invoke();  // Log Result
        }

        private void UpdateAll()
        {
            TagDirectory[] tagDirectories = TagDirectory.GetAllTagDirectories(ConfigManager.Configuration);

            foreach (TagDirectory tagDirectory in tagDirectories)
            {
                UpdateSpecific(tagDirectory);
            }
        }

        private static Action LogUpdateProgress(int idCount, TagDirectory directory)
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
