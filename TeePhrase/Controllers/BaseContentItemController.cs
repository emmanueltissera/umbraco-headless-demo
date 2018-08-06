using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;
using TeePhrase.ViewResults;
using Umbraco.Headless.Client.Models;
using Umbraco.Headless.Client.Services;
using Umbraco.Headless.Client.Web;

namespace TeePhrase.Controllers
{
    public class BaseContentItemController : DefaultUmbracoController
    {
        public BaseContentItemController(UmbracoContext umbracoContext, HeadlessService headlessService) : base(umbracoContext, headlessService)
        {
        }

        public Task<IActionResult> RenderContentItem<T>() where T : ContentItem, new()
        {
            // get the content for the current route
            var content = UmbracoContext.GetContent(false);

            if (content == null)
            {
                return Show404();
            }

            // map the ContentItem to the requested model
            var model = HeadlessService.MapTo<T>(content);            

            // return the view from the given location
            return Task.FromResult((IActionResult)View("~/Views/DefaultUmbraco/Index.cshtml", model));
        }

        public Task<IActionResult> Show404()
        {
            var content = new ContentItem() { Name = "404 - Not Found" };
            return Task.FromResult((IActionResult)new NotFoundViewResult("~/Views/Error/NotFound.cshtml", new ViewDataDictionary(ViewData) { Model = content }));
        }
    }
}
