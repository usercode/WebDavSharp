using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace WebDavSharp.Core.WebDAV.Results
{
    public abstract class WebDavXmlResult : IWebDavXmlResult
    {
        protected XNamespace dav = "DAV:";

        public virtual int StatusCode => (int)HttpStatusCode.MultiStatus;

        public abstract XElement ToXml(WebDavContext context);
    }
}
