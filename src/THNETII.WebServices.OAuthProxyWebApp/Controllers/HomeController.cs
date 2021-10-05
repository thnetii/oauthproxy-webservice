using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;

using THNETII.WebServices.OAuthProxyWebApp.Models;

namespace THNETII.WebServices.OAuthProxyWebApp.Controllers
{
    [Route("~/")]
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        [HttpGet]
        [HttpGet("[action]")]
        public IActionResult Index() => View();

        [HttpGet("[action]")]
        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("[action]")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
