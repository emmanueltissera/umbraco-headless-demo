using System.Collections.Generic;
using Umbraco.Headless.Client.Models;

namespace TeePhrase.Models
{
    public class Design : ContentItem
    {
        public string Phrase { get; set; }

        public string WhoSaidThis { get; set; }

        public List<MediaItem> Thumbnail { get; set; }

        public List<MediaItem> Photos { get; set; }

        public string Price { get; set; }

        public string Description { get; set; }

        public string ReferenceNumber { get; set; }

    }
}
