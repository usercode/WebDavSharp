using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace WebDavSharp.Core.WebDAV
{
    public interface IWebDavXmlResult : IWebDavResult
    {
        XElement ToXml(WebDavContext context);
    }
}
