using Rule34.us.Downloader.Logic.Commands;

namespace Rule34.us.Downloader.Logic.Tags
{
    public class Tags
    {
        public string[] UserInput { get; private set; }
        public string[]? ShadowTags { get; private set; }
        public string[] Raw => TrimmedInput().Concat(ShadowTags ?? Array.Empty<string>()).ToArray();

        public Tags(string userInput, string[]? shadowTags = null)
        {
            if (userInput is null)
            {
                throw new ArgumentNullException(nameof(userInput));
            }

            UserInput = Format(userInput);
            ShadowTags = shadowTags;
        }

        public Tags(string[] userTags, string[]? shadowTags = null)
        {
            if (userTags is null)
            {
                throw new ArgumentNullException(nameof(userTags));
            }

            UserInput = userTags;
            ShadowTags = shadowTags;
        }

        private string[] Format(string tags)
        {
            return tags.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(tag => tag.Trim()).ToArray();
        }

        public string[] TrimmedInput()
        {
            return UserInput.Any() && UserInput[0].StartsWith(Command.prefix) ? UserInput.Skip(1).ToArray() : UserInput;
        }

        public void SetShadowTags(string[] tags)
        {
            ShadowTags = tags;
        }

        public override string ToString()
        {
            return string.Join(" ", TrimmedInput());
        }

    }
}
