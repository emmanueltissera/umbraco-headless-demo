using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TeePhrase.Models;
using Umbraco.Headless.Client.Models;
using Umbraco.Headless.Client.Services;

namespace TeePhrase.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public HomeController(HeadlessService headlessService)
        {
            this._headlessService = headlessService;
        }

        private readonly HeadlessService _headlessService;

        public async Task<IActionResult> Headless()
        {
            // Get all content
            var allContent = await _headlessService.Query().GetAll();            
            return View(allContent);
        }
    }
}
