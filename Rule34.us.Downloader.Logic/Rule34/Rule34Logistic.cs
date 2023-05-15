using HtmlAgilityPack;
using Rule34.us.Downloader.Logic.Extensions;
using Rule34.us.Downloader.Logic.Utility;

namespace Rule34.us.Downloader.Logic.Rule34
{
    internal class Rule34Logistic
    {
        private readonly Func<string, string> LINK_IMAGE = (id) => $@"https://rule34.us/index.php?r=posts/view&id={id}";
        private readonly Func<string[], int, string> LINK_PAGE = (tags, pageCount) => $@"https://rule34.us/index.php?r=posts/index&q={tags.TagJoin("all")}&page={pageCount}";
        private readonly WebUtilities web = new();
        private readonly HtmlDocument doc = new();
        private HtmlNode? contentChild;

        /// <summary>
        /// Get all Ids with the specified Tags.
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="pages"></param>
        /// <returns></returns>
        public string[] GetAllIdsByTags(Tags.Tags tags)
        {
            int pages = 0;
            List<string> ids = new();
            string[]? requestedIds;

            do
            {
                requestedIds = web.Request(LINK_PAGE(tags.Raw, pages));

                if (requestedIds == null || !requestedIds.Any())
                {
                    break;
                }

                ids.AddRange(requestedIds);
                pages++;

            } while (true);

            return ids.ToArray();
        }

        /// <summary>
        /// Get all Ids with the specified Tags until a specific id.
        /// /// </summary>
        /// <param name="id"></param>
        /// <param name="tags"></param>
        /// <param name="pages"></param>
        /// <returns></returns>
        public string[] GetAllIdsByTagsTill(string id, Tags.Tags tags)
        {
            int pages = 0;
            List<string> ids = new();
            string[]? requestedIds;

            do
            {
                requestedIds = web.Request(LINK_PAGE(tags.Raw, pages));

                if (requestedIds == null || !requestedIds.Any())
                {
                    break;
                }

                // Run: 1 / Time: 25422
                // Run: 2 / Time: 21307
                // Run: 3 / Time: 17115
                // Run: 4 / Time: 19074
                // Run: 5 / Time: 22905
                // Average Time: 21164
                if (!requestedIds.Select(rq => int.Parse(rq)).Min().ToString().IsBiggerThan(id))
                {
                    ids.AddRange(requestedIds.TakeWhile(x => x.IsBiggerThan(id)));
                    break;
                }
                ids.AddRange(requestedIds);

                pages++;

            } while (true);

            return ids.ToArray();
        }

        /// <summary>
        /// Convert all given ids into an image link.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Dictionary<string, string> ConvertIdsToLinks(string[] ids)
        {
            Dictionary<string, string> links = new();
            int runs = 1;
            int totalIds = ids.Count();
            string link;

            if (!ids.Any())
            {
                return links;
            }

            foreach (string id in ids)
            {
                Logger.LogSimple($"[{runs}/{totalIds}] {LINK_IMAGE(id)}\n"); // Log

                web.LoadHTMLDocWithLink(doc, LINK_IMAGE(id));

                contentChild = doc.DocumentNode.SelectSingleNode("//div[@class='content_push']")
                                               .FirstChild.NextSibling;

                link = contentChild.Name == "img"
                    ? contentChild.GetAttributeValue<string>("src", "n/a")
                    : contentChild.ChildNodes[1].GetAttributeValue<string>("src", "n/a");

                links.Add(id, link);
                runs++;
            }

            return links;
        }

        /// <summary>
        /// Download all given links.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="idLinkPairs"></param>
        /// <returns></returns>
        public Task Download(string path, Dictionary<string, string> idLinkPairs)
        {
            List<Task> tasks = new();
            KeyValuePair<string, string> kvp;

            if (!idLinkPairs.Any())
            {
                return Task.CompletedTask;
            }

            for (int i = 0; i < idLinkPairs.Count(); i++)
            {
                kvp = idLinkPairs.ElementAt(i);
                tasks.Add(WebUtilities.Download(kvp.Value, Path.Combine(path, kvp.Key)));
            }

            int x = Console.CursorLeft;
            int y = Console.CursorTop;
            int tasksCount = tasks.Count();

            Task.Run(async () =>
            {
                int total = 0;

                while (tasks.Any())
                {
                    Task finishedTask = await Task.WhenAny(tasks);
                    _ = tasks.Remove(finishedTask);
                    total++;
                    Logger.LogSimpleAt($"Downloading [{total}/{tasksCount}]", x, y, ConsoleColor.Yellow);
                }

                Console.WriteLine();
            }).Wait();

            return Task.CompletedTask;
        }
    }
}
