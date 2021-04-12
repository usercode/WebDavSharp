using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebDavSharp.Core.Builders
{
    public interface IWebDavBuilder
    {
        IServiceCollection Services { get; }
    }
}
