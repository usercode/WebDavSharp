using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebDavSharp.Core.WebDAV;
using WebDavSharp.Core.WebDAV.Results;

namespace WebDavSharp.Middlewares
{
    public class WebDavLockResult : WebDavXmlResult
    {
        public WebDavLockResult()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public override XElement ToXml(WebDavContext context)
        {
            return new XElement(dav + "prop",
                new XAttribute(XNamespace.Xmlns + "d", dav),
                        new XElement(dav + "lockdiscovery",
                            new XElement(dav + "activelock",
                                new XElement(dav + "lockscope",
                                    new XElement(dav + "exclusive")),
                                new XElement(dav + "locktype",
                                    new XElement(dav + "write")),
                                new XElement(dav + "depth", "Infinity"),
                                new XElement(dav + "timeout", "Second-604800"),
                                new XElement(dav + "locktoken",
                                    new XElement(dav + "href", $"opaquelocktoken:{Id}")))
                ));
        }
    }
}
