namespace Rule34.us.Downloader.Logic.Tags
{
    public class TagAnalyzer
    {
        private string? tags;

        public TagAnalyzer(string? tags)
        {
            this.tags = tags;
        }

        public List<string> Analyze()
        {
            if (tags == null)
                throw new ArgumentNullException("tags");



            Logger.LogSimple($"searching tags: [{HighlightTags(tags)}]\n");

            return tags.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(tag => tag.Trim()).ToList();
        }

        private string HighlightTags(string? tags)
        {
            return string.Join(" ", tags.Split(' ').Select(tag => $"'{tag}'"));
        }
    }
}
