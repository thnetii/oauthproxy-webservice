using Microsoft.AspNetCore.Mvc;

namespace THNETII.WebServices.OAuthProxyWebApp.AuthorizeUI.Components
{
    [ViewComponent(Name = NameGuid)]
    public class AuthorizeUIMainViewComponent : ViewComponent
    {
        public const string NameGuid = "0dcd503a-ae38-4929-a1ef-ec2589da2baf";

        public IViewComponentResult Invoke() => View();
    }
}
