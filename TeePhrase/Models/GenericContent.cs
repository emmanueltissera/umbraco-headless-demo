using Newtonsoft.Json.Linq;
using TeePhrase.Models.Grid;
using Umbraco.Headless.Client.Models;

namespace TeePhrase.Models
{
    public class GenericContent : ContentItem
    {
        public UmbracoGrid BodyContent { get; set; }
    }
}
