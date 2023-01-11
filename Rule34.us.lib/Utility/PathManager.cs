namespace Rule34.us.Downloader.Utility
{
    internal struct PathManager
    {
        public static string Pictures { get; private set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
        public static string Documents { get; private set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

        public static string PathInDocuments(string file) => Path.Combine(Documents, file);
        public static string PathInPictures(string file) => Path.Combine(Pictures, file);
    }
}
