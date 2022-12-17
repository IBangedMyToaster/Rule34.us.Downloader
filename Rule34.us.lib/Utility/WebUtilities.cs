using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Rule34.us.Downloader.Utility
{
    internal class WebUtilities
    {
        internal async Task SaveFile(string url, string filename)
        {
            using (var client = new WebClient())
            {
                client.DownloadFileTaskAsync(new Uri(url), $"{filename}{Path.GetExtension(url)}");
            }
        }

        internal List<string>? Request(string link)
        {
            List<string> ids;

            HtmlDocument htmlDocument = new HtmlDocument();
            LoadHTMLDocWithLink(htmlDocument, link);

            try
            {
                ids = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='thumbail-container']").ChildNodes
                                               .SelectMany(p => p.ChildNodes.Select(l => l.Id.ToString()))
                                               .ToList();
            }
            catch (System.NullReferenceException)
            {
                return null;
            }

            return ids;
        }

        internal void LoadHTMLDocWithLink(HtmlDocument htmlDocument, string link)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(link);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                htmlDocument.Load(reader);
            }
        }
    }
}
