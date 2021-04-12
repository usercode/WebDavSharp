using System;
using System.Collections.Generic;
using System.Text;

namespace WebDavSharp.Core.WebDAV
{
    /// <summary>
    /// WebDavContext
    /// </summary>
    public class WebDavContext
    {
        public WebDavContext(string baseUrl, string path, DepthMode depth)
        {
            BaseUrl = baseUrl;
            Path = path.UrlDecode();
            Depth = depth;
        }

        /// <summary>
        /// BaseUrl
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Path
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Depth
        /// </summary>
        public DepthMode Depth { get; set; }
    }
}
