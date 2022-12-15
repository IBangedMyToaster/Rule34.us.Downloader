using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Security;

namespace Rule34.us.Downloader
{
    public class MainViewViewModel
    {
        private Logger _logger;
        private List<string> tags;
        private List<string> ids = new List<string>();
        private const string imageHoster = "img2.rule34.us";
        private const string videoHoster = "video.rule34.us";

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

        public MainViewViewModel(List<string> tags, Logger logger)
        {
            this._logger = logger;
            this.tags = tags;
            RetrieveIdsByTags();

            if (!ids.Any())
                return;

            //string uri;

            //ids.ForEach(id =>
            //{
            //    uri = DownloadId(id);
            //    SaveImage(uri, id, GetImageFormat(Path.GetExtension(uri)));
            //    Console.WriteLine("Saved Image");
            //});

            string configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "rule34.json");
            Config conf = new Config(configPath, false);

            string tagsDirectory = Path.Combine(conf.savePath, String.Join("_", tags));
            Directory.CreateDirectory(tagsDirectory);

            Dictionary<string, string> links = DownloadMultiple(ids.ToArray());
            links.ToList().ForEach(kvp => SaveFile(kvp.Value, Path.Combine(tagsDirectory, kvp.Key)));

            //foreach(var link in links)
            //    SaveImage(link.Value, link.Key, GetImageFormat(link.Value));

            #region Testing

            //Dictionary<string, int> stats = new Dictionary<string, int>();
            //foreach (var link in links)
            //{
            //    string key = Path.GetExtension(link.Value);

            //    if (stats.ContainsKey(key))
            //        stats[key]++;
            //    else
            //        stats.Add(key, 1);
            //}
            //Console.WriteLine(String.Join("\n", stats.Select(kvp => $"{kvp.Key} | {kvp.Value}")));
            //File.WriteAllLines("testLinks.txt", links.Keys);

            #endregion

            _logger.Log("Saved Image", LogLevel.Information);
        }

        private string DownloadId(string id)
        {
            string link;

            HtmlDocument doc = new HtmlDocument();
            HtmlNode contentChild;

            LoadHTMLDocWithLink(doc, _getImageById(id));

            contentChild = doc.DocumentNode.SelectSingleNode("//div[@class='content_push']")
                                            .FirstChild.NextSibling;

            if (contentChild.Name == "img")
                link = contentChild.GetAttributeValue<string>("src", "n/a");
            else
                link = contentChild.ChildNodes[1].GetAttributeValue<string>("src", "n/a");

            return link;
        }

        private Dictionary<string, string> DownloadMultiple(string[] ids)
        {
            Dictionary<string, string> links = new Dictionary<string, string>();
            
            HtmlDocument doc = new HtmlDocument();
            HtmlNode contentChild;
            int counter = 1;

            foreach(string id in ids)
            {
                LoadHTMLDocWithLink(doc, _getImageById(id));

                contentChild = doc.DocumentNode.SelectSingleNode("//div[@class='content_push']")
                                               .FirstChild.NextSibling;

                if (contentChild.Name == "img")
                    links.Add(id, contentChild.GetAttributeValue<string>("src", "n/a"));
                else
                    links.Add(id, contentChild.ChildNodes[1].GetAttributeValue<string>("src", "n/a"));

                _logger.Log($"Collected number {counter}", LogLevel.Information);
                counter++;
            }

            return links;
        }

        private void SaveFile(string url, string filename)
        {
            using (var client = new WebClient())
            {
                client.DownloadFileAsync(new Uri(url), $"{filename}{Path.GetExtension(url)}");
            }
        }

        public void SaveImage(string imageUrl, string filename, ImageFormat format)
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(imageUrl);
            Bitmap bitmap; bitmap = new Bitmap(stream);

            Directory.CreateDirectory("Result");

            if (bitmap != null)
            {
                bitmap.Save($"Result/{filename}.{format.ToString().ToLower()}", format);
            }

            stream.Flush();
            stream.Close();
            client.Dispose();
        }
        
        private ImageFormat GetImageFormat(string? extension) => extension switch
        {
            ".jpeg" => ImageFormat.Jpeg,
            ".png" => ImageFormat.Png,
            ".jpg" => ImageFormat.Jpeg,
            ".gif" => ImageFormat.Gif,
            _ => throw new NotImplementedException()
        };

        public void RetrieveIdsByTags()
        {
            long counter = 0;

            do
            {
                List<string>? request = Request(_link(tags, counter));

                if (!request.Any() || request == null)
                    break;

                ids.AddRange(request);
                Console.WriteLine($"Collected Page {counter + 1}, total elements: {ids.Count()}");
                counter++;

            } while (true);

            Console.WriteLine($"{counter} pages found");
        }

        private List<string>? Request(string link)
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

        private void LoadHTMLDocWithLink(HtmlDocument htmlDocument, string link)
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
