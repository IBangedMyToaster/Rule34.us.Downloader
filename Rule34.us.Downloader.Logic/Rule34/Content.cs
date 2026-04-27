namespace Rule34.us.Downloader.Logic.Rule34
{
    public class Content
    {
        public string Id { get; set; }
        public string Url { get; set; } = null!;
        public Uri Uri { get { return new Uri(Url); } }
        public Uri Referer { get; set; } = null!;

        public string Filename { get { return $"{Id}{Path.GetExtension(Url)}"; } }

        public Content(string id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }
    }
}
