namespace Rule34.us.Downloader.Logic.Extensions
{
    public static class StringExtensions
    {
        public static bool ContainsInsensitive(this List<string> value, string stringToCompare)
        {
            return value.Any(val => val.ToLower().Contains(stringToCompare.ToLower()));
        }

        [Obsolete]
        public static bool Contains(this string value, IEnumerable<string> list)
        {
            return list.All(item => value.Contains(item));
        }

        public static string[] Trim(this string[] array)
        {
            return array.Select(x => x.Trim()).ToArray();
        }

        public static string Remove(this string value, string rmString)
        {
            return value.Replace(rmString, string.Empty);
        }

        public static string ReplaceMulti(this string value, string oldChar1, string newChar1, string oldChar2, string newChar2)
        {
            return value.Replace(oldChar1, newChar1).Replace(oldChar2, newChar2);
        }

        public static string TagJoin(this string[] array, string fallback)
        {
            return array.Any() ? string.Join("+", array) : fallback;
        }

        public static bool IsBiggerThan(this string val1, string val2)
        {
            return int.Parse(val1) > int.Parse(val2);
        }
    }
}
