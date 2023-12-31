﻿using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ResidenceMocker;
using ResidenceMocker.Mockers;
using HostEntity = ResidenceMocker.Entities.Host;
using Host = Microsoft.Extensions.Hosting.Host;

var builder = Host.CreateApplicationBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();

builder.Services.AddResidenceServices(configuration);

using var host = builder.Build();

var mocker = host.Services.GetService<IMocker>();
Debug.Assert(mocker != null, nameof(mocker) + " != null");
await mocker.MockAsync();

await host.RunAsync();