using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using WebDavSharp.Core.WebDAV;

namespace WebDavSharp.Core
{
    public static class HttpContextExtensions
    {
        public static WebDavContext GetWebDavContext(this HttpContext httpContent)
        {
            WebDavContext? webDavContext = (WebDavContext?)httpContent.Items["WebDavContext"];

            if (webDavContext == null)
            {
                throw new Exception("WebDAV context was not found.");
            }

            return webDavContext;
        }

        public static void SetWebDavContext(this HttpContext httpContext, WebDavContext webDavContext)
        {
            httpContext.Items.Add("WebDavContext", webDavContext);
        }
    }
}
