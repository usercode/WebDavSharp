using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDavSharp.Middlewares;
using WebDavSharp.Core.Locking;

namespace WebDavSharp.Locking
{
    public interface IWebDavLockProvider
    {
        Task<WebDavLockResult> LockAsync(WebDavLockScope scope, string path);

        Task UnlockAsync(string lockToken);
    }
}
