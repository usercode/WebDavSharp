using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using WebDavSharp.Core.WebDAV.Results;

namespace WebDavSharp.Core.WebDAV
{
    class WebDavCollectionsResult : WebDavXmlResult
    {
        public WebDavCollectionsResult(DirectoryInfo directory)
        {
            Directory = directory;
        }

        public DirectoryInfo Directory { get; }

        public override XElement ToXml(WebDavContext context)
        {
            return new XElement(dav + "multistatus",
                new XElement(dav + "response",
                    new XElement(dav + "href", $"{context.BaseUrl}/"),
                    new XElement(dav + "propstat",
                        new XElement(dav + "prop",
                            new XElement(dav + "displayname", Directory.Name),
                            new XElement(dav + "resourcetype",
                                new XElement(dav + "collection"))),
                        new XElement(dav + "status", "HTTP/1.1 200 OK")
                        )),
                context.Depth == DepthMode.One ? Directory.GetDirectories()
                                                    .Select(x => new WebDavCollectionResult(x).ToXml(context))
                                                    .ToArray()
                                                    .Concat(
                                                    Directory
                                                    .GetFiles()
                                                    .OrderBy(x => x.Name)
                                                    .Select(x => new WebDavFile(x).ToXml(context))
                                                    .ToArray()) : new XElement[0]
                );
        }
    }
}
