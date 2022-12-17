using HtmlAgilityPack;
using System.Drawing.Imaging;
using System.Net;

namespace Rule34.us.Downloader
{
    public class MainViewViewModel
    {
        private Logger logger;
        private string command;
        private List<string> tags;
        private List<string> ids;
        private FileManager fileManager;

        public MainViewViewModel(List<string> tags, Logger logger)
        {
            this.logger = logger;
            this.tags = tags;

            this.fileManager = new FileManager(logger);
            DownloadTags(tags);
        }

        public MainViewViewModel(string command, List<string> tags, Logger logger)
        {
            this.command = command;
            this.tags = tags;
            this.logger = logger;

            this.fileManager = new FileManager(logger);

            GetActionByCommand(command, tags).Invoke();
        }

        private Action GetActionByCommand(string command, List<string> tags) => command switch
        {
            "--update" => () => UpdateAllFiles(tags),
            _ => throw new NotImplementedException(command)
        };

        public void DownloadTags(List<string> tags)
        {
            var ids = fileManager.RetrieveIdsByTags(tags);
            fileManager.DownloadFiles(ids, tags);
        }

        public void UpdateAllFiles(List<string> tags)
        {
            string id;
            List<TagDirectory> directories = fileManager.GetFoldersWithTags(tags).Select(path => new TagDirectory(path)).ToList();

            foreach (var directory in directories)
            {
                id = fileManager.GetLastIdFromFolder(directory.OriginalPath);
                var ids = fileManager.RetrieveIdsByTags(directory.Tags, id);

                if(!ids.Any())
                {
                    logger.LogSimple($"Directory {directory.Name} is UpToDate!\n", ConsoleColor.Green);
                    continue;
                }

                fileManager.DownloadFiles(ids, tags, directory.OriginalPath);
                logger.LogSimple($"Updated {directory.Name}\n", ConsoleColor.Green);
            }
        }
    }
}
