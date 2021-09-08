using System.Net.Mime;

using Microsoft.AspNetCore.Mvc;

namespace THNETII.WebServices.OAuthProxyWebApp.AuthorizeUI.Controllers
{
    public class AuthorizeController : Controller
    {
        [HttpGet]
        public ViewResult Index() => View();
    }
}
