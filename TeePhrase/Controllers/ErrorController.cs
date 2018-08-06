using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using TeePhrase.ViewResults;
using Umbraco.Headless.Client.Models;

namespace TeePhrase.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error/{statusCode}")]
        public ViewResult Status(int statusCode)
        {
            HttpContext.Response.StatusCode = statusCode;

            if (statusCode != StatusCodes.Status404NotFound)
            {
                return View("~/Views/Error/GeneralError.cshtml");
            }

            var content = new ContentItem() { Name = "404 - Not Found" };
            return new NotFoundViewResult("~/Views/Error/NotFound.cshtml", new ViewDataDictionary(ViewData) { Model = content });
        }
    }
}
