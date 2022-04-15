using Microsoft.AspNetCore.Builder;
using WebDavSharp.Core;
using WebDavSharp.Core.WebDAV;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<WebDavOptions>(builder.Configuration.GetSection("General"));
builder.Services.Configure<LocalFilesystemOptions>(builder.Configuration.GetSection("Filesystem"));

builder.Services.AddWebDavSharp(x => x.IsReadOnly = false)
                .AddFilesystem();

var app = builder.Build();

var env = app.Services.GetRequiredService<IWebHostEnvironment>();

if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseWebDavSharp(PathString.Empty);

app.Run();