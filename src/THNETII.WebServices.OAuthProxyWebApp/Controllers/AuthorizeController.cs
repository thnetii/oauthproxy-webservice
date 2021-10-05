using Microsoft.AspNetCore.Mvc;

using THNETII.WebServices.OAuthProxyWebApp.AuthorizeUI.Controllers;

namespace THNETII.WebServices.OAuthProxyWebApp.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    public class AuthorizeController : AuthorizeBaseController { }
}
