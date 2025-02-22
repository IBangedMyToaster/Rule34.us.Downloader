﻿using Rule34.us.Downloader.Logic.Extensions;
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
                if (!TagDirectory.Exists(ConfigManager.Configuration, Tags))
                {
                    Logger.LogSimple($"The folder \"{(string.Join(" ", Tags.TrimmedInput()))}\" does not exist!\n", ConsoleColor.Red);
                    return;
                }

                CompleteSpecific(TagDirectory.GetTagDirectoryByTags(ConfigManager.Configuration, Tags));
                return;
            }

            CompleteAll();
        }

        private void CompleteSpecific(TagDirectory tagDirectory)
        {
            // Get all ids by tags
            Logger.LogSimple($"Completing {string.Join(" ", tagDirectory.Tags.TrimmedInput())}...\n", ConsoleColor.Yellow); // Log Checking
            List<Content> contentList = logistic.GetAllIdsByTags(tagDirectory.Tags);

            // Compare existing Folder with Links and Filter doubles
            contentList = contentList.RemoveContentWithIds(tagDirectory.GetFilenames());

            // Get all links by ids
            logistic.GetLinks(contentList);

            // Download all files by links and save in folder
            _ = logistic.Download(tagDirectory.OriginalPath, contentList);

            LogUpdateProgress(contentList.Count(), tagDirectory).Invoke();  // Log Result
        }

        private void CompleteAll()
        {
            TagDirectory[] tagDirectories = TagDirectory.GetAllTagDirectories(ConfigManager.Configuration);

            foreach (TagDirectory tagDirectory in tagDirectories)
            {
                CompleteSpecific(tagDirectory);
            }
        }

        private static Action LogUpdateProgress(int idCount, TagDirectory directory)
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
