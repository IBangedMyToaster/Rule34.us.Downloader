namespace Rule34.us.Downloader
{
    public class GlobalTags
    {
        public List<string> Tags { get; set; }

        public GlobalTags(List<string> tags)
        {
            this.Tags = tags;
        }

        public GlobalTags()
        {
            this.Tags = new List<string>();
        }

        public void Remove(string[] tags)
        {
            foreach (var tag in tags.Select(tag => tag.Trim()))
            {
                this.Tags.RemoveAll(t => t.ToLower() == tag.ToLower());
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

        internal void SortTags() => this.Tags = Tags.OrderBy(tag => tag).ToList();

        internal void RemoveDuplicats()
        {
            this.Tags = this.Tags.Select(tag => tag.ToLower()).Distinct().ToList();
        }
    }
}
