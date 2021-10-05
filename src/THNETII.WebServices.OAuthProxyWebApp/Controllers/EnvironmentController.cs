using System;
using System.Collections.Generic;
using System.Net.Mime;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using THNETII.WebServices.OAuthProxyWebApp.Services;

namespace THNETII.WebServices.OAuthProxyWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnvironmentController : ControllerBase
    {
        [HttpGet("[action]")]
        [EnvironmentFilter(nameof(Environments.Development))]
        [Produces(MediaTypeNames.Text.Plain, MediaTypeNames.Application.Json)]
        public ActionResult<string> Configuration(
            [FromServices] IConfiguration config
            )
        {
            var configRoot = config as IConfigurationRoot;
            return Ok(configRoot?.GetDebugView());
        }

        [HttpGet("[action]")]
        [EnvironmentFilter(nameof(Environments.Development))]
        [Produces(MediaTypeNames.Text.Plain, MediaTypeNames.Application.Json)]
        public ActionResult<string> WebRootFiles(
            [FromServices] IWebHostEnvironment environment
            )
        {
            if (environment is null)
                return Ok();

            var uriBuilder = new UriBuilder
            {
                Scheme = Request.Scheme,
                Host = Request.Host.Host
            };
            var fileProvider = environment.WebRootFileProvider;
            List<string> filePaths = new();
            Queue<Uri> pathStack = new(new[] { uriBuilder.Uri });
            while (pathStack.TryDequeue(out var currentPath))
            {
                var contents = fileProvider.GetDirectoryContents(currentPath.GetComponents(UriComponents.Path, UriFormat.SafeUnescaped).TrimEnd('/'));
                if (contents?.Exists != true)
                    continue;
                foreach (var fileInfo in contents)
                {
                    if (fileInfo.IsDirectory)
                        pathStack.Enqueue(new Uri(currentPath, fileInfo.Name + '/'));
                    else
                        filePaths.Add($"{new Uri(currentPath, fileInfo.Name).GetComponents(UriComponents.Path, UriFormat.SafeUnescaped)}: {fileInfo.PhysicalPath}");
                }
            }

            return Ok(string.Join("\r\n", filePaths));
        }
    }
}
