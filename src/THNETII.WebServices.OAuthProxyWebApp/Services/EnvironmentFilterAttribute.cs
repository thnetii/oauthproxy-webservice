using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace THNETII.WebServices.OAuthProxyWebApp.Services
{
    public sealed class EnvironmentFilterAttribute : ActionFilterAttribute
    {
        public EnvironmentFilterAttribute(string? name = default)
        {
            Name = name;
        }

        public string? Name { get; }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));
            var env = context.HttpContext.RequestServices
                .GetService<IWebHostEnvironment>();

            if (env is not null && Name is { Length: > 0 } && !env.IsEnvironment(Name))
            {
                const string notAllowedErrorMessage = "Action is not allowed to execute in current environment.\n";
                if (context.Controller is not ControllerBase controller)
                    throw new InvalidOperationException(notAllowedErrorMessage);
                context.Result = controller.BadRequest(notAllowedErrorMessage);
                return Task.CompletedTask;
            }

            return base.OnActionExecutionAsync(context, next);
        }
    }
}
