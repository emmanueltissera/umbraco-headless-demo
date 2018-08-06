using Umbraco.Headless.Client.Models;

namespace TeePhrase.Models
{
    public class UniqueSellingPoint : ContentItem
    {
        public string Icon { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ButtonTitle { get; set; }

        public ContentItem ButtonLink { get; set; }

    }
}
