namespace Rule34.us.Downloader.Extensions
{
    public static class CharExtensions
    {
        public static bool Matches(this Char value, char matchValue)
        {
            char val = Char.ToLower(value);

            return val == matchValue;
        }
    }
}
