using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebDavSharp.Core.Builders
{
    class WebDavBuilder : IWebDavBuilder
    {
        public WebDavBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
