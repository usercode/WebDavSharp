using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WebDavSharp.Core.Filesystems;
using WebDavSharp.Core.WebDAV;

namespace WebDavSharp.Core
{
    public interface IWebDavFilesystem
    {
        Task<IWebDavResult> FindPropertiesAsync(WebDavContext context);

        Task<IWebDavResult> PatchPropertiesAsync(WebDavContext context);

        Task<IWebDavResult> CreateCollectionAsync(WebDavContext context);

        Task<Stream> OpenFileStreamAsync(WebDavContext context);

        Task WriteFileAsync(Stream stream, WebDavContext context);

        Task<bool> DeleteAsync(WebDavContext context);

        Task<bool> MoveToAsync(WebDavContext context, string path);

        
    }
}
