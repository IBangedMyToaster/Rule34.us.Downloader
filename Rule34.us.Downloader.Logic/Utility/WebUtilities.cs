using HtmlAgilityPack;
using Rule34.us.Downloader.Logic.Extensions;
using System.Net;

namespace Rule34.us.Downloader.Logic.Utility
{
    public class WebUtilities
    {
        private string[]? ids;
        private readonly HtmlDocument htmlDocument = new();
        HttpClient? httpClient;

        public static async Task Download(string url, string filepath)
        {
            using HttpClient client = new();
            await client.DownloadFileTaskAsync(new Uri(url), $"{filepath}{Path.GetExtension(url)}");
        }

        internal string[]? Request(string link)
        {
            LoadHTMLDocWithLink(htmlDocument, link);

            try
            {
                ids = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='thumbail-container']").ChildNodes
                                               .SelectMany(p => p.ChildNodes.Select(l => l.Id.ToString())).ToArray();
            }
            catch (NullReferenceException)
            {
                return null;
            }

            return ids;
        }

        internal void LoadHTMLDocWithLink(HtmlDocument htmlDocument, string link)
        {
            httpClient = new HttpClient();
            using Stream stream = httpClient.GetStreamAsync(link).GetAwaiter().GetResult();
            using StreamReader reader = new(stream);
            htmlDocument.Load(reader);

            //request = (HttpWebRequest)WebRequest.Create(link);

            //using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //using Stream stream = response.GetResponseStream();
            //using StreamReader reader = new(stream);
            //htmlDocument.Load(reader);
        }
    }
}
