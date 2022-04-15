using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebDavSharp.Core.WebDAV;
using WebDavSharp.Core.WebDAV.Results;

namespace WebDavSharp.Core
{
    /// <summary>
    /// LocalFilesystem
    /// </summary>
    class LocalFilesystem : IWebDavFilesystem
    {
        public LocalFilesystem(IOptions<LocalFilesystemOptions> options)
        {
            Options = options.Value;
        }

        /// <summary>
        /// Options
        /// </summary>
        public LocalFilesystemOptions Options { get; }

        private string GetLocalPath(string path)
        {
            if (path == null)
            {
                path = "";
            }

            if(path.StartsWith("/"))
            {
                path = path.Substring(1);
            }

            string fullpath = Path.Combine(Options.SourcePath, path);

            if (fullpath.StartsWith(Options.SourcePath) == false)
            {
                throw new Exception("Invalid path");
            }

            return fullpath;
        }

        public async Task<Stream> OpenFileStreamAsync(WebDavContext context)
        {
            FileInfo file = new FileInfo(GetLocalPath(context.Path));

            return file.OpenRead();
        }

        public async Task WriteFileAsync(Stream stream, WebDavContext context)
        {
            FileInfo file = new FileInfo(GetLocalPath(context.Path));

            using (Stream fs = file.OpenWrite())
            {
                //clear content
                fs.SetLength(0);

                await stream.CopyToAsync(fs);
            }
        }

        public async Task<IWebDavResult> FindPropertiesAsync(WebDavContext context)
        {
            string fullpath = GetLocalPath(context.Path);

            //is directory
            if (Directory.Exists(fullpath))
            {
                return new WebDavCollectionsResult(new DirectoryInfo(fullpath));
            }
            //is file
            else if (File.Exists(fullpath))
            {
                return new WebDavFile(new FileInfo(fullpath));
            }
            //not found
            else
            {
                return new WebDavFileNotFoundResult();
            }
        }

        public async Task<IWebDavResult> PatchPropertiesAsync(WebDavContext context)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(WebDavContext context)
        {
            string fullpath = GetLocalPath(context.Path);

            if (Directory.Exists(fullpath))
            {
                Directory.Delete(fullpath, true);
            }
            else if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
            }
            else
            {
                return false;
            }

            return true;
        }

        public async Task<IWebDavResult> CreateCollectionAsync(WebDavContext context)
        {
            string fullpath = GetLocalPath(context.Path);

            DirectoryInfo dir = Directory.CreateDirectory(fullpath);

            return new WebDavNoContentResult(HttpStatusCode.Created);
        }

        public async Task<bool> MoveToAsync(WebDavContext context, string path)
        {
            string localPathFrom = GetLocalPath(context.Path);
            string localPathTo = GetLocalPath(path);
            
            Directory.Move(localPathFrom, localPathTo);

            return true;
        }
    }
}
