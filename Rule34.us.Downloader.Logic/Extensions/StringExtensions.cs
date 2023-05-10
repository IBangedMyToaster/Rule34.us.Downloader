namespace Rule34.us.Downloader.Logic.Extensions
{
    public static class StringExtensions
    {
        public static bool ContainsInsensitive(this List<string> value, string stringToCompare)
        {
            return value.Any(val => val.ToLower().Contains(stringToCompare.ToLower()));
        }

        public static bool Contains(this string value, IEnumerable<string> list)
        {
            return list.All(item => value.Contains(item));
        }

        public static string[] Trim(this string[] array)
        {
            return array.Select(x => x.Trim()).ToArray();
        }
    }
}
