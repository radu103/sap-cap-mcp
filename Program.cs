﻿using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    // Windows-specific configuration
    Console.WriteLine("Running on Windows");
    
    builder.Services.AddMcpServer()
        .WithStdioServerTransport()
        .WithToolsFromAssembly()
        .WithPromptsFromAssembly();
}
else
{
    // Additional Linux-specific configuration
    Console.WriteLine("Running on Linux x64");

    builder.Services.AddMcpServer()
        .WithToolsFromAssembly()
        .WithPromptsFromAssembly();
}

builder.Logging.AddConsole()
    .AddDebug()
    .AddEventSourceLogger()
    .AddFilter("Microsoft", LogLevel.Warning)
    .AddFilter("System", LogLevel.Warning)
    .AddFilter("Default", LogLevel.Trace);

var app = builder.Build(); // by default, this will start as stdio server

// add /sse endpoint with HTTP GET 
// and 
// add /message endpoint with HTTP POST
app.MapMcp(); 

await app.RunAsync();