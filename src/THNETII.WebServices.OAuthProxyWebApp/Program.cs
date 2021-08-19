using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace THNETII.WebServices.OAuthProxyWebApp
{
    public static class Program
    {
        public static void Main(string[]? args) =>
            CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[]? args) =>
            Host.CreateDefaultBuilder(args ?? Array.Empty<string>())
                .ConfigureWebHostDefaults(wb => wb.UseStartup(ctx => new Startup(ctx)));
    }
}
