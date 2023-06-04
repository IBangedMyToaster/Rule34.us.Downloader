using Rule34.us.Downloader.Logic.Rule34;

namespace Rule34.us.Downloader.Logic.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool Matches(this IEnumerable<string> collection, string value)
        {
            return collection.Any(x => x == value);
        }

        public static List<Content> RemoveContentWithIds(this List<Content> contentList, IEnumerable<string> existingFiles)
        {
            return contentList.Where(content => !existingFiles.Contains(content.Id)).ToList();
        }
    }
}
