using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace TeePhrase.ViewResults
{
    public class NotFoundViewResult : ViewResult
    {
        public NotFoundViewResult(string viewName, ViewDataDictionary viewDataDictionary)
        {
            ViewName = viewName;
            ViewData = viewDataDictionary;
            StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
}
