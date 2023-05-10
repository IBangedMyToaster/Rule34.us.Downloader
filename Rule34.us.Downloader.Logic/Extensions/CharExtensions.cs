namespace Rule34.us.Downloader.Logic.Extensions
{
    public static class CharExtensions
    {
        public static bool Matches(this char value, char matchValue)
        {
            char val = char.ToLower(value);

            return val == matchValue;
        }
    }
}
