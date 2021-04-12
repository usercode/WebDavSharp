using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebDavSharp.Middlewares;
using WebDavSharp.Core;
using WebDavSharp.Core.Filesystems;
using WebDavSharp.Core.WebDAV;

namespace WebDavSharp.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<WebDavOptions>(Configuration.GetSection("General"));
            services.Configure<LocalFilesystemOptions>(Configuration.GetSection("Filesystem"));

            services.AddWebDavSharp(x => x.IsReadOnly = false)
                    .AddFilesystem()
                ;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapWebDavSharp();
            });
        }
    }
}
