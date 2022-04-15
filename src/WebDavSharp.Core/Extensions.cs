using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Runtime.CompilerServices;
using WebDavSharp.Middlewares;
using WebDavSharp.Core.Builders;
using WebDavSharp.Core.Filesystems;
using WebDavSharp.Core.WebDAV;

namespace WebDavSharp.Core
{
    public static class Extensions
    {
        public static IWebDavBuilder AddWebDavSharp(this IServiceCollection services, Action<WebDavOptions>? options = null)
        {
            if (options != null)
            {
                services.Configure(options);
            }

            return new WebDavBuilder(services);
        }

        public static IWebDavBuilder AddFilesystem(this IWebDavBuilder builder, Action<LocalFilesystemOptions>? options = null)
        {
            if (options != null)
            {
                builder.Services.Configure(options);
            }

            builder.Services.AddTransient<IWebDavFilesystem, LocalFilesystem>();

            return builder;
        }

        public static IApplicationBuilder UseWebDavSharp(this IApplicationBuilder builder, PathString basePath)
        {
            builder.Map(basePath, b =>
            {
                b.UseRouting();
                b.UseEndpoints(endpoints => endpoints.MapWebDavSharp());
            });

            return builder;
        }

        private static IEndpointConventionBuilder MapWebDavSharp(this IEndpointRouteBuilder endpoints)
        {
            IOptions<WebDavOptions> options = endpoints.ServiceProvider.GetRequiredService<IOptions<WebDavOptions>>();

            RequestDelegate pipeline = endpoints.CreateApplicationBuilder()
                                                         .UseMiddleware<WebDavMiddleware>()
                                                         .Build();

            return endpoints.Map("{*filePath}", pipeline).WithDisplayName("WebDavSharp");
        }
    }
}
