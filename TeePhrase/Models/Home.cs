using System.Collections.Generic;
using Umbraco.Headless.Client.Models;

namespace TeePhrase.Models
{
    public partial class Home : ContentItem
    {
        public string Title { get; set; }

        public string Lead { get; set; }

        public string PhraseSamples { get; set; }

        public List<MediaItem> CarouselImages { get; set; }

        public List<UniqueSellingPoint> UniqueSellingPoints { get; set; }
    }
}
