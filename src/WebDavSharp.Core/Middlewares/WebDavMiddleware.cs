using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using WebDavSharp.Locking;
using WebDavSharp.Core;
using WebDavSharp.Core.WebDAV;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebDavSharp.Middlewares
{
    /// <summary>
    /// WebDavMiddleware
    /// </summary>
    class WebDavMiddleware
    {
        private readonly RequestDelegate _next;

        public WebDavMiddleware(RequestDelegate next,
            //IWebDavLockProvider lockProvider,
            IOptions<WebDavOptions> options,
            IWebDavFilesystem filesystem,
            ILogger<WebDavMiddleware> logger)
        {
            Options = options.Value;

            _next = next;

            //LockProvider = lockProvider;
            Filesystem = filesystem;
            Logger = logger;
        }

        /// <summary>
        /// Options
        /// </summary>
        public WebDavOptions Options { get; }

        /// <summary>
        /// Logger
        /// </summary>
        public ILogger<WebDavMiddleware> Logger { get; }

        /// <summary>
        /// LockProvider
        /// </summary>
        //private IWebDavLockProvider LockProvider { get; }

        /// <summary>
        /// Filesystem
        /// </summary>
        private IWebDavFilesystem Filesystem { get; }

        public async Task InvokeAsync(HttpContext context)
        {
            WebDavContext webDavContext = new WebDavContext(
                                                baseUrl : $"{context.Request.Scheme}://{context.Request.Host}",
                                                path: (string?)context.GetRouteValue("filePath") ?? string.Empty,
                                                depth: context.Request.Headers["Depth"].FirstOrDefault() switch
                                                {
                                                    "0" => DepthMode.Zero,
                                                    "1" => DepthMode.One,
                                                    "infinity" => DepthMode.Infinity,
                                                    _ => DepthMode.None
                                                });

            context.SetWebDavContext(webDavContext);

            Logger.LogTrace($"HTTP Request: {context.Request.Method} {context.Request.Path}");

            Task action =  context.Request.Method switch
            {
                "OPTIONS" => ProcessOptionsAsync(context),
                "PROPFIND" => ProcessPropfindAsync(context),
                "PROPPATCH" => ProcessPropPatchAsync(context),
                "MKCOL" => ProcessMKCOLAsync(context),
                "GET" => ProcessGetAsync(context),
                "PUT" => ProcessPutAsync(context),
                "HEAD" => ProcessHeadAsync(context),
                "LOCK" => ProcessLockAsync(context),
                "UNLOCK" => ProcessUnlockAsync(context),
                "MOVE" => ProcessMoveAsync(context),
                "DELETE" => ProcessDeleteAsync(context),
                _ => ProcessUnknown(context)
            };

            await action;
        }

        private async Task ProcessOptionsAsync(HttpContext context)
        {
            if (Options.IsReadOnly)
            {
                context.Response.Headers.Add("Allow", "OPTIONS, TRACE, GET, HEAD, PROPFIND");
            }
            else
            {
                context.Response.Headers.Add("Allow", "OPTIONS, TRACE, GET, HEAD, DELETE, PUT, POST, COPY, MOVE, MKCOL, PROPFIND, PROPPATCH, LOCK, UNLOCK");
            }
            
            context.Response.Headers.Add("DAV", "1, 2");
            context.Response.Headers.Add("MS-Author-Via", "DAV");
        }

        private async Task ProcessGetAsync(HttpContext context)
        {
            using (Stream fs = await Filesystem.OpenFileStreamAsync(context.GetWebDavContext()))
            {
                await fs.CopyToAsync(context.Response.Body);
            }
        }

        private async Task ProcessMKCOLAsync(HttpContext context)
        {
            if (Options.IsReadOnly)
            {
                throw new Exception();
            }

            await Filesystem.CreateCollectionAsync(context.GetWebDavContext());
        }

        private Task ProcessPutAsync(HttpContext context)
        {
            if (Options.IsReadOnly)
            {
                throw new Exception();
            }

            return Filesystem.WriteFileAsync(context.Request.Body, context.GetWebDavContext());
        }

        private async Task ProcessHeadAsync(HttpContext context)
        {

        }

        private async Task ProcessPropPatchAsync(HttpContext context)
        {
            if (Options.IsReadOnly)
            {
                throw new Exception();
            }

            var reader = new StreamReader(context.Request.Body);

            string request = await context.Request.ReadContentAsString();


        }

        private async Task ProcessPropfindAsync(HttpContext context)
        {
            IWebDavResult result = await Filesystem.FindPropertiesAsync(context.GetWebDavContext());

            context.Response.StatusCode = result.StatusCode;

            if (result is IWebDavXmlResult xmlResult)
            {
                XElement xml = xmlResult.ToXml(context.GetWebDavContext());

                if (xml != null)
                {
                    context.Response.ContentType = "application/xml";

                    await context.Response.WriteAsync(xml.ToString());
                }
            }
        }

        private async Task ProcessLockAsync(HttpContext context)
        {
            if (Options.IsReadOnly)
            {
                throw new Exception();
            }

            string request = await context.Request.ReadContentAsString();

            string t = new WebDavLockResult().ToXml(context.GetWebDavContext()).ToString();

            await context.Response.WriteAsync(t);
        }

        private async Task ProcessUnlockAsync(HttpContext context)
        {
            if (Options.IsReadOnly)
            {
                throw new Exception();
            }
        }

        private Task ProcessDeleteAsync(HttpContext context)
        {
            if (Options.IsReadOnly)
            {
                throw new Exception();
            }

            return Filesystem.DeleteAsync(context.GetWebDavContext());
        }

        private async Task ProcessMoveAsync(HttpContext context)
        {
            if (Options.IsReadOnly)
            {
                throw new Exception();
            }

            if (context.Request.Headers.TryGetValue("destination", out StringValues destinations) == false
                || destinations.Any() == false)
            {
                throw new Exception("No destination header available.");
            }

            Uri newUri = new Uri(destinations.First());

            await Filesystem.MoveToAsync(context.GetWebDavContext(), newUri.PathAndQuery.UrlDecode());

            context.Response.StatusCode = (int)StatusCodes.Status201Created;
        }

        private async Task ProcessUnknown(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
        }

    }
}
