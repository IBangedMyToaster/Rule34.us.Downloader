using HtmlAgilityPack;
using Rule34.us.Downloader.Extensions;
using Rule34.us.Downloader.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Rule34.us.Downloader
{
    internal class FileManager
    {
        private Logger logger;
        private WebUtilities web = new WebUtilities();
        private const string imageHoster = "img2.rule34.us";
        private const string videoHoster = "video.rule34.us";
        Config conf = new Config(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "rule34.json"), false);

        private Func<string, string> _getImageById = (id) =>
        {
            return $@"https://rule34.us/index.php?r=posts/view&id={id}";
        };

        private Func<List<string>, long, string> _link = (tagList, pageNum) =>
        {
            string tags = tagList.Any() ? String.Join("+", tagList) : "all";
            string page = $"&page={pageNum}";

            return $@"https://rule34.us/index.php?r=posts/index&q={tags}{page}";
        };


        public FileManager(Logger logger)
        {
            this.logger = logger;
        }

        #region Update

        internal List<string> GetFoldersWithTags(List<string> tags)
        {
            if (!tags.Any())
                return Directory.GetDirectories(conf.savePath).ToList();

            return Directory.GetDirectories(conf.savePath).Where(dir => new DirectoryInfo(dir).Name.Contains(tags)).ToList();
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

        private void UpdateFiles(List<string> tags)
        {
            if (tags.Any())
            {
                Console.WriteLine("Update specific");
                return;
            }

            Console.WriteLine("Update All");

            string[] dirs = Directory.GetDirectories(conf.savePath);

            foreach (var dir in dirs)
            {
                
            }
        }

        #endregion

        #region Download

        public void DownloadFiles(List<string> ids, List<string> tags, string tagFolder = null)
        {
            if (!ids.Any())
                return;

            if(tagFolder == null)
                tagFolder = Path.Combine(conf.savePath, String.Join(" & ", tags));

            Directory.CreateDirectory(tagFolder);

            int pageCounter = 1;
            Dictionary<string, string> links = DownloadMultiple(ids.ToArray(), ref pageCounter);
            links.ToList().ForEach(kvp => web.SaveFile(kvp.Value, Path.Combine(tagFolder, kvp.Key)));
        }

        private Dictionary<string, string> DownloadMultiple(string[] ids, ref int pageCounter)
        {
            Dictionary<string, string> links = new Dictionary<string, string>();


            HtmlDocument doc = new HtmlDocument();
            HtmlNode contentChild;
            int runs = 1;
            bool newPage = true;
            int amountOfIds = ids.Count();

            foreach (string id in ids)
            {
                if (newPage)
                {
                    logger.LogSimple($"collecting files from page = {pageCounter}\n", ConsoleColor.White);
                    newPage = false;
                }

                web.LoadHTMLDocWithLink(doc, _getImageById(id));
                logger.LogSimple($"[{runs}/{amountOfIds}] {_getImageById(id)}\n");

                contentChild = doc.DocumentNode.SelectSingleNode("//div[@class='content_push']")
                                               .FirstChild.NextSibling;

                if (contentChild.Name == "img")
                    links.Add(id, contentChild.GetAttributeValue<string>("src", "n/a"));
                else
                    links.Add(id, contentChild.ChildNodes[1].GetAttributeValue<string>("src", "n/a"));

                if (runs / pageCounter == 42)
                {
                    PrintCollectingStats(pageCounter, links);
                    pageCounter++;
                    newPage = true;
                }
                runs++;
            }

            PrintCollectingStats(pageCounter, links);
            pageCounter++;

            return links;
        }

        public List<string> RetrieveIdsByTags(List<string> tags, string id = null)
        {
            logger.LogSimple("\nsearching...\n\n");
            logger.LogSimple("searching for image links...\n" +
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

            logger.LogSimple($"Found {counter} pages with {ids.Count()} elements.\n\n", ConsoleColor.White);
            return ids;
        }

        #endregion

        #region Utility

        private void PrintCollectingStats(int pageCounter, Dictionary<string, string> links)
        {
            logger.LogSimple($"page {pageCounter} done! ", ConsoleColor.White);
            logger.LogSimple($"[ total file count: {links.Count()} ; images: {GetAmountOf(links, Format.Image)} ; videos: {GetAmountOf(links, Format.Video)} ]\n\n", ConsoleColor.Yellow);
            pageCounter++;
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
