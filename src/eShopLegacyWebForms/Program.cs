using Autofac;
using Autofac.Extensions.DependencyInjection;
using eShopLegacyWebForms.Models;
using eShopLegacyWebForms.Models.Infrastructure;
using eShopLegacyWebForms.Modules;
using eShopLegacyWebForms.Services;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;

[assembly: XmlConfigurator(ConfigFile = "log4Net.xml")]

var builder = WebApplication.CreateBuilder(args);

// Configure log4net
var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4Net.xml"));

// Load configuration
var connectionString = builder.Configuration.GetConnectionString("CatalogDBContext");
var useMockData = bool.Parse(builder.Configuration["AppSettings:UseMockData"] ?? "false");

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new ApplicationModule(useMockData, builder.Configuration));
});

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

// Register CatalogDBContext in DI for non-mock mode
if (!useMockData)
{
    builder.Services.AddDbContext<CatalogDBContext>(options =>
        options.UseSqlServer(connectionString));
}

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePagesWithReExecute("/Error");
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseSession();

// Set session values on first request (replaces Session_Start in Global.asax)
app.Use(async (context, next) =>
{
    if (context.Session.GetString("MachineName") == null)
    {
        context.Session.SetString("MachineName", Environment.MachineName);
        context.Session.SetString("SessionStartTime", DateTime.Now.ToString());
    }
    await next();
});

// Set log4net thread context properties (replaces Application_BeginRequest in Global.asax)
app.Use(async (context, next) =>
{
    log4net.ThreadContext.Properties["activityid"] = System.Diagnostics.Activity.Current?.Id ?? Guid.NewGuid().ToString();
    log4net.ThreadContext.Properties["requestinfo"] = context.Request?.Path.Value + ", " + context.Request?.Headers["User-Agent"].ToString();
    await next();
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    if (!useMockData)
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDBContext>();
        dbContext.Database.Migrate();

        var initializer = scope.ServiceProvider.GetRequiredService<CatalogDBInitializer>();
        initializer.Seed(dbContext);
    }
}

app.Run();
