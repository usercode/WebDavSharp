using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace WebDavSharp.Core.WebDAV.Results
{
    public class WebDavFileNotFoundResult : WebDavXmlResult
    {
        public override int StatusCode => (int)HttpStatusCode.NotFound;

        public override XElement ToXml(WebDavContext context)
        {
            return new XElement("NotFound");
        }
    }
}
