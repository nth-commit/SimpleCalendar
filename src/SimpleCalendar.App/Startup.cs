using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SimpleCalendar.Framework;
using SimpleCalendar.Utility.Configuration;

namespace SimpleCalendar.App
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IEnvironmentSettingsFactory, EnvironmentSettingsFactory>();

            services.AddConfigurationServices(_hostingEnvironment.EnvironmentName);
            services.ConfigureFromProvider<Auth0AuthOptions>("Auth0");

            if (_hostingEnvironment.IsDevelopment())
            {
                services.AddCors();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            IEnvironmentSettingsFactory environmentSettingsFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseCors(opts => opts
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .AllowCredentials());

                app.Use(async (context, next) =>
                {
                    if (context.Request.Path == "/config")
                    {
                        context.Response.ContentType = "application/json";
                        var envSettings = environmentSettingsFactory.CreateJson(context);
                        await context.Response.WriteAsync(envSettings);
                    }
                    else
                    {
                        await next.Invoke();
                    }
                });
            }
            else
            {
                app.UseRewriter(new RewriteOptions().AddRedirectToHttpsPermanent());

                app.UseStaticFiles();

                app.Run(context =>
                {
                    context.Response.ContentType = "text/html";

                    using (var fs = File.OpenRead("wwwroot/index.html"))
                    {
                        var doc = new HtmlDocument();
                        doc.Load(fs);

                        var appSettingsScript = doc.CreateElement("script");
                        appSettingsScript.AppendChild(doc.CreateTextNode(
                            $"var ENVIRONMENT_SETTINGS = {environmentSettingsFactory.CreateJson(context)}"));

                        var head = doc.DocumentNode.SelectSingleNode("/html/head");
                        head.AppendChild(appSettingsScript);

                        doc.Save(context.Response.Body);
                    }

                    return Task.FromResult(0);
                });
            }
        }
    }
}
