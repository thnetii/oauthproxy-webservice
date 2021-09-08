using System;
using System.IO;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace THNETII.WebServices.OAuthProxyWebApp
{
    public class Startup
    {
        private const string SwaggerUrlName = "current";

        public OpenApiInfo OpenApiInfo { get; } = new()
        {
            Title = "OAuth HTTP Proxy",
            Version = typeof(Startup).Assembly.GetName()?.Version?.ToString(2)
        };

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private readonly IConfiguration configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(this);
            services.AddMvc();
            services.AddRazorPages();
            services.AddControllersWithViews();
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
                configuration.GetSection(nameof(Microsoft.ApplicationInsights))
            );
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

#if DEBUG
            app.UseBrowserLink();
#endif

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
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
