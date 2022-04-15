using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WebDavSharp.Core
{
    public class LocalFilesystemOptions
    {
        public LocalFilesystemOptions()
        {
            SourcePath = "";
        }

        public string SourcePath { get; set; }
    }
}
