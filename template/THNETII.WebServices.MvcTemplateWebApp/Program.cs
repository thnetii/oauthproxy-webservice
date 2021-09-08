using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

[assembly: CLSCompliant(false)]

namespace THNETII.WebServices.MvcTemplateWebApp
{
    public static class Program
    {
        public static Task<int> Main(string[]? args)
        {
            RootCommand rootCommand = new()
            {
                Handler = CommandHandler.Create(
                    (IHost host, CancellationToken cancelToken) =>
                        host.RunAsync(cancelToken)
                )
            };
            var parser = new CommandLineBuilder(rootCommand)
                .UseDefaults()
                .UseHost(CreateHostBuilder)
                .Build();
            return parser.InvokeAsync(args ?? Array.Empty<string>());
        }

        public static IHostBuilder CreateHostBuilder(string[]? args) =>
            Host.CreateDefaultBuilder(args ?? Array.Empty<string>())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
