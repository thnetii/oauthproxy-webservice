using Microsoft.AspNetCore.Mvc;

namespace THNETII.WebServices.OAuthProxyWebApp.Controllers
{
    using AuthorizeControllerBase = AuthorizeUI.Controllers.AuthorizeController;

    [Route("authorize")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AuthorizeController : AuthorizeControllerBase { }
}
