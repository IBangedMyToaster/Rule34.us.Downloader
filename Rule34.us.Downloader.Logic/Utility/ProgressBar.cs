namespace Rule34.us.Downloader.Logic.Utility
{
    internal class ProgressBar
    {
        private readonly int progressBarMax = 30;
        private readonly char character = '█';
        public int CustomMax { get; set; }

        public ProgressBar(int max)
        {
            CustomMax = max;
        }

        internal void Update(int progress, string stats)
        {
            int filled = (int)(progress / (double)CustomMax * progressBarMax);
            int empty = progressBarMax - filled;

            string filledBar = new string(character, filled);
            string emptyBar = new string(character, empty);

            Logger.LogMultipleOnSpot(filledBar, emptyBar, stats, 32, ConsoleColor.Blue, ConsoleColor.DarkGray, ConsoleColor.White);
        }
    }
}
