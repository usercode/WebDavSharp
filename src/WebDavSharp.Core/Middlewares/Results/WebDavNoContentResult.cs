using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace WebDavSharp.Core.WebDAV.Results
{
    public class WebDavNoContentResult : IWebDavResult
    {
        public WebDavNoContentResult(HttpStatusCode statusCode)
        {
            StatusCode = (int)statusCode;
        }

        public virtual int StatusCode { get; }
    }
}
