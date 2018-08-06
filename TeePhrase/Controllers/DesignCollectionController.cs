using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TeePhrase.Models;
using Umbraco.Headless.Client.Services;
using Umbraco.Headless.Client.Web;

namespace TeePhrase.Controllers
{
    public class DesignCollectionController : BaseContentItemController
    {
        public DesignCollectionController(UmbracoContext umbracoContext, HeadlessService headlessService) : base(umbracoContext, headlessService)
        {
        }

        public override Task<IActionResult> Index()
        {
            return RenderContentItem<DesignCollection>();
        }
    }
}
