namespace Rule34.us.Downloader.Logic.Tags
{
    public class GlobalTags
    {
        public List<string> Tags { get; set; }

        public GlobalTags(List<string> tags)
        {
            Tags = tags;
        }

        public GlobalTags()
        {
            Tags = new List<string>();
        }

        public void Remove(string[] tags)
        {
            foreach (var tag in tags.Select(tag => tag.Trim()))
            {
                Tags.RemoveAll(t => t.ToLower() == tag.ToLower());
            }
        }

        public void AddRange(string[] tags)
        {
            foreach (var tag in tags.Select(tag => tag.Trim()))
            {
                if (Tags.Any(t => t == tag))
                {
                    Logger.LogSimple($"{tag} is already in the list!", ConsoleColor.Red);
                    continue;
                }

                Tags.Add(tag);
            }
        }

        internal List<string> GetTags()
        {
            return Tags.Select(tag => tag.ToLower().Trim()).ToList();
        }

        internal void PrintTags()
        {
            foreach (var tag in Tags)
                Logger.LogSimple(tag + "\n");
        }

        internal void SortTags() => Tags = Tags.OrderBy(tag => tag).ToList();

        internal void RemoveDuplicats()
        {
            Tags = Tags.Select(tag => tag.ToLower()).Distinct().ToList();
        }
    }
}
