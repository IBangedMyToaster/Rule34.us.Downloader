using HtmlAgilityPack;
using Rule34.us.Downloader.Logic.Extensions;
using Rule34.us.Downloader.Logic.Utility;

namespace Rule34.us.Downloader.Logic.Rule34
{
    internal class Rule34Logistic
    {
        private readonly Func<string, string> LINK_IMAGE = (id) => $@"https://rule34.us/index.php?r=posts/view&id={id}";
        private readonly Func<string[], int, string> LINK_PAGE = (tags, pageCount) => $@"https://rule34.us/index.php?r=posts/index&q={tags.TagJoin("all")}&page={pageCount}";
        private readonly WebUtilities webUtilities = new();

        /// <summary>
        /// Get all Ids with the specified Tags.
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="pages"></param>
        /// <returns></returns>
        public List<Content> GetAllIdsByTags(Tags.Tags tags, string? id = null)
        {
            int pages = 0;
            List<Content> contentList = new List<Content>();
            string[]? requestedIds;

            do
            {
                requestedIds = webUtilities.Request(LINK_PAGE(tags.Raw, pages));

                if (requestedIds == null || !requestedIds.Any())
                {
                    break;
                }

                // Used for Updating
                if (id != null && !requestedIds.Select(rq => int.Parse(rq)).Min().ToString().IsBiggerThan(id))
                {
                    contentList.AddRange(requestedIds.TakeWhile(x => x.IsBiggerThan(id)).Select(id => new Content(id)));
                    break;
                }

                contentList.AddRange(requestedIds.Select(id => new Content(id)));
                pages++;

            } while (true);

            return contentList;
        }

        /// <summary>
        /// Convert all given ids into an image link.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<Content> GetLinks(List<Content> contentList)
        {
            Dictionary<string, string> links = new();
            int totalCount = contentList.Count();
            Task[] tasks = new Task[totalCount];
            int runs = 1;

            if (!contentList.Any())
            {
                return contentList;
            }

            foreach (Content content in contentList)
            {
                Logger.LogSimple($"[{runs}/{totalCount}] {LINK_IMAGE(content.Id)}\n"); // Log

                tasks[runs - 1] = GetDownloadLinkAsync(content);

                runs++;
            }

            ProgressBar progressBar = new ProgressBar(totalCount);
            Logger.LogSimple("\nGetting Links\n");
            tasks.ForEachCompleted((finished, total) => progressBar.Update(finished, $"{finished} Files / {total} Files"));
            Console.WriteLine();

            return contentList;
        }

        /// <summary>
        /// Adds the link pointing to the Content with the given ID to links.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="links"></param>
        /// <returns></returns>
        private Task GetDownloadLinkAsync(Content content)
        {
            HttpClient client = new HttpClient();
            HtmlDocument doc = new HtmlDocument();

            return Task.Run(async () =>
            {
                string html = await client.GetStringAsync(LINK_IMAGE(content.Id));
                doc.LoadHtml(html);

                var element = doc.DocumentNode.SelectSingleNode("//div[@class='content_push']").FirstChild.NextSibling;

                content.Url = element.Name == "img"
                            ? element.GetAttributeValue<string>("src", "n/a")
                            : element.ChildNodes[1].GetAttributeValue<string>("src", "n/a");
            });
        }

        /// <summary>
        /// Download all given links.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="idLinkPairs"></param>
        /// <returns></returns>
        public Task Download(string path, List<Content> contentList)
        {
            Task[] tasks = new Task[contentList.Count()];

            if (!contentList.Any())
            {
                return Task.CompletedTask;
            }

            Logger.LogSimple("\nDownloading Content\n");

            for (int i = 0; i < contentList.Count(); i++)
            {
                tasks[i] = WebUtilities.Download(path, contentList.ElementAt(i));
            }

            ProgressBar progressBar = new ProgressBar(contentList.Count());
            tasks.ForEachCompleted((finished, total) => progressBar.Update(finished, $"{finished} Files / {total} Files"));
            Console.WriteLine();
            //tasks.ForEachCompleted((finished, total) => Logger.LogOnSpot($"Downloading [{finished}/{total}]", ConsoleColor.Yellow));

            return Task.CompletedTask;
        }
    }
}
