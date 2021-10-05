using System;
using System.Linq;
using System.Reflection;

[assembly: CLSCompliant(false)]

namespace THNETII.WebServices.OAuthProxyWebApp.AuthorizeUI
{
    public static class AssemblyInfo
    {
        public static string? StaticWebAssetBasePath { get; } =
            typeof(AssemblyInfo).Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .Where(attr => attr.Key == nameof(StaticWebAssetBasePath))
            .FirstOrDefault()?.Value;

        public static string AppName { get; } =
            $"{nameof(AuthorizeUI)} - {nameof(OAuthProxyWebApp)}";
    }
}
