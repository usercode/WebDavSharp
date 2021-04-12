using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace WebDavSharp.Core.WebDAV
{
    public interface IWebDavResult
    {
        int StatusCode { get; }
    }
}
