using Microsoft.AspNetCore.Mvc;

namespace THNETII.WebServices.OAuthProxyWebApp.AuthorizeUI.Controllers
{
    public abstract class AuthorizeBaseController : Controller
    {
        public const string NameGuid = "0dcd503a-ae38-4929-a1ef-ec2589da2baf";

        [HttpGet]
        [HttpGet("[action]")]
        public IActionResult Index() => View(viewName: NameGuid + "/Index");
    }
}
