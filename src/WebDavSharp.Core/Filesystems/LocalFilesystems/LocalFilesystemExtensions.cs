using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDavSharp.Core
{
    public static class LocalFilesystemExtensions
    {
        public static string GetLocalPath(this LocalFilesystemOptions options, string path)
        {
            if (path == null)
            {
                path = "";
            }

            if (path.StartsWith("/"))
            {
                path = path.Substring(1);
            }

            string fullpath = Path.Combine(options.SourcePath, path);

            if (fullpath.StartsWith(options.SourcePath) == false)
            {
                throw new Exception("Invalid path");
            }

            return fullpath;
        }
    }
}
