namespace Rule34.us.Downloader.Logic.Utility
{
    internal struct PathManager
    {
        public static string Pictures { get; private set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
        public static string AppData { get; private set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ScrewsTools", "Rule34.us");

        public static string PathInAppData(string file)
        {
            Directory.CreateDirectory(AppData);
            return Path.Combine(AppData, file);
        }

        public static string PathInPictures(string file)
        {
            return Path.Combine(Pictures, file);
        }
    }
}
