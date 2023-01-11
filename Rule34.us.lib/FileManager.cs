using HtmlAgilityPack;
using Rule34.us.Downloader.Extensions;
using Rule34.us.Downloader.Utility;

namespace Rule34.us.Downloader
{
    internal class FileManager
    {
        private WebUtilities web = new WebUtilities();
        private Config config;

        private Func<string, string> _getImageById = (id) => $@"https://rule34.us/index.php?r=posts/view&id={id}";

        private Func<List<string>, long, string> _link = (tagList, pageNum) =>
        {
            string tags = tagList.Any() ? String.Join("+", tagList) : "all";
            string page = $"&page={pageNum}";

            return $@"https://rule34.us/index.php?r=posts/index&q={tags}{page}";
        };

        public FileManager(Config config)
        {
            this.config = config;
        }

        #region Update

        internal List<string> GetFoldersWithTags(List<string> tags)
        {
            if (!tags.Any())
                return Directory.GetDirectories(config.SavePath).ToList();

            return new List<string>() { Directory.GetDirectories(config.SavePath).First(dir => new DirectoryInfo(dir).Name.Contains(tags)) };
        }

        internal string GetLastIdFromFolder(string path)
        {
            List<string> tags = new DirectoryInfo(path).Name.Split("&").Select(tag => tag.Trim()).ToList();
            string lastId = Directory.GetFiles(path).Select(path => Path.GetFileNameWithoutExtension(path)).OrderBy(id => long.Parse(id)).ToList().LastOrDefault();

            if (lastId == null)
            {
                Console.WriteLine("No Files found");
                return null;
            }

            return lastId;
        }

        #endregion

        #region Download

        public string CheckTagFolder(string[] ids, List<string> tags, string tagFolder = null)
        {
            if (!ids.Any())
                return null;

            if (tagFolder == null)
                tagFolder = Path.Combine(config.SavePath, String.Join(" & ", tags));

            Directory.CreateDirectory(tagFolder);

            return tagFolder;
        }

        public Dictionary<string, string> DownloadMultiple(string[] ids, string tagFolder)
        {
            Dictionary<string, string> links = new Dictionary<string, string>();

            WebUtilities web = new WebUtilities();
            HtmlDocument doc = new HtmlDocument();
            HtmlNode contentChild;
            int runs = 1;
            int amountOfIds = ids.Count();
            string link;

            foreach (string id in ids)
            {
                web.LoadHTMLDocWithLink(doc, _getImageById(id));
                Logger.LogSimple($"[{runs}/{amountOfIds}] {_getImageById(id)}\n");

                contentChild = doc.DocumentNode.SelectSingleNode("//div[@class='content_push']")
                                               .FirstChild.NextSibling;

                if (contentChild.Name == "img")
                    link = contentChild.GetAttributeValue<string>("src", "n/a");
                else
                    link = contentChild.ChildNodes[1].GetAttributeValue<string>("src", "n/a");

                links.Add(id, link);
                web.SaveFile(link, Path.Combine(tagFolder, id));
                runs++;
            }

            PrintCollectingStats(links);
            return links;
        }

        public string[] RetrieveIdsByTags(List<string> tags, string id = null)
        {
            Logger.CrashLog("Debug.txt", String.Join(" ", tags));

            Logger.LogSimple("\nsearching...\n\n");
            Logger.LogSimple("searching for image links...\n" +
                              "This Process might take 2-3 mins to complete.\n" +
                              "please be patient..\n\n", ConsoleColor.DarkCyan);
            long counter = 0;
            List<string> ids = new List<string>();
            List<string>? requestedIds;

            do
            {
                requestedIds = web.Request(_link(tags, counter));

                if (requestedIds == null || !requestedIds.Any())
                    break;

                if (id != null && requestedIds.Contains(id))
                {
                    ids.AddRange(requestedIds.Where(element => long.Parse(element) > long.Parse(id)));
                    counter++;
                    break;
                }

                ids.AddRange(requestedIds);
                counter++;

            } while (true);

            Logger.LogSimple($"Found {counter} pages with {ids.Count()} elements.\n\n", ConsoleColor.White);
            return ids.ToArray();
        }

        #endregion

        #region Utility

        private void PrintCollectingStats(Dictionary<string, string> links)
        {
            Logger.LogSimple($"[ total file count: {links.Count()} ; images: {GetAmountOf(links, Format.Image)} ; videos: {GetAmountOf(links, Format.Video)} ]\n\n", ConsoleColor.Yellow);
        }

        private object GetAmountOf(Dictionary<string, string> links, Format format)
        {
            return links.Where(link => GetImageFormat(Path.GetExtension(link.Value)) == format).Count();
        }

        private Format GetImageFormat(string? extension) => extension switch
        {
            ".jpeg" or ".png" or ".jpg" => Format.Image,
            ".gif" or ".mp4" => Format.Video,
            _ => throw new NotImplementedException()
        };

        #endregion
    }
}
