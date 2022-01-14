// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Linq;
using IdentityServerAspNetIdentity;
using Microsoft.AspNetCore.Builder;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    // uncomment to write to Azure diagnostics stream
    //.WriteTo.File(
    //    @"D:\home\LogFiles\Application\identityserver.txt",
    //    fileSizeLimitBytes: 1_000_000,
    //    rollOnFileSizeLimit: true,
    //    shared: true,
    //    flushToDiskInterval: TimeSpan.FromSeconds(1))
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
    .CreateLogger();

try
{
    var seed = args.Contains("/seed");
    if (seed)
    {
        args = args.Except(new[] { "/seed" }).ToArray();
    }

    var builder = WebApplication.CreateBuilder(args);
    builder.Host
        .UseSerilog()
        .ConfigureAppConfiguration(c => c.AddEnvironmentVariables().AddCommandLine(args));

    Startup.ConfigureServices(builder);

    var app = builder.Build();
    Startup.Configure(app);

    if (seed)
    {
        Log.Information("Seeding database...");
        var config = app.Services.GetRequiredService<IConfiguration>();
        var connectionString = config.GetConnectionString("DefaultConnection");
        SeedData.EnsureSeedData(connectionString);
        Log.Information("Done seeding database");
        return 0;
    }

    Log.Information("Starting host...");
    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
