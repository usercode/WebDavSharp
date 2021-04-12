# WebDavSharp
WebDAV middleware for ASP.NET Core

### How to use ###

```csharp
using WebDavSharp.Core;

public void Configure(IApplicationBuilder app)
{
    services.Configure<WebDavOptions>(Configuration.GetSection("General"));
    services.Configure<LocalFilesystemOptions>(Configuration.GetSection("Filesystem"));

    services.AddWebDavSharp(x => x.IsReadOnly = false)
            .AddFilesystem();
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapWebDavSharp();
    });
}

```
