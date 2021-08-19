using System;
using System.IO;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace THNETII.WebServices.OAuthProxyWebApp
{
    public class Startup
    {
        private const string SwaggerUrlName = "current";
        private readonly WebHostBuilderContext context;

        public OpenApiInfo OpenApiInfo { get; }

        public Startup(WebHostBuilderContext context) : base()
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));

            OpenApiInfo = new()
            {
                Title = "OAuth HTTP Proxy",
                Version = GetType().Assembly.GetName()?.Version?.ToString(2)
            };
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(this);
            services.AddRazorPages();
            services.AddControllers()
                .AddJsonOptions(json =>
                {
                    json.JsonSerializerOptions.Converters.Add(
                        new JsonStringEnumConverter()
                    );
                });
            services.AddSwaggerGen(swaggerConfig =>
            {
                swaggerConfig.SwaggerDoc(SwaggerUrlName, OpenApiInfo);
                swaggerConfig.IncludeXmlComments(Path.Combine(
                    AppContext.BaseDirectory,
                    GetType().Assembly.GetName()?.Name + ".xml"
                ));
            });
#if DEBUG
            services.AddApplicationInsightsTelemetry(
                context.Configuration.GetSection(nameof(Microsoft.ApplicationInsights))
            );
#endif
        }

        public void Configure(IApplicationBuilder app)
        {
            var env = context.HostingEnvironment;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // app.UseExceptionHandler("/error");
#if DEBUG
                app.UseBrowserLink();
#endif
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseSwaggerUI(swaggerConfig =>
            {
                swaggerConfig.SwaggerEndpoint(
                    $"/swagger/{SwaggerUrlName}/swagger.json",
                    OpenApiInfo.Title
                );
            });

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapSwagger();
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
