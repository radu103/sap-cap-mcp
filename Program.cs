using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMcpServer()
    .WithStdioServerTransport()    
    .WithToolsFromAssembly()
    .WithPromptsFromAssembly();

builder.Logging.AddConsole()
    .AddDebug()
    .AddEventSourceLogger()
    .AddFilter("Microsoft", LogLevel.Warning)
    .AddFilter("System", LogLevel.Warning)
    .AddFilter("WeatherTool", LogLevel.Debug)
    .AddFilter("Default", LogLevel.Trace);

builder.Services.AddSingleton(_ =>
{
    var client = new HttpClient() { BaseAddress = new Uri("https://api.weather.gov") };
    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("weather-tool", "1.0"));
    return client;
});

var app = builder.Build(); // by default, this will start as stdio server

// add /sse endpoint with HTTP GET 
// and 
// add /message endpoint with HTTP POST
app.MapMcp(); 

await app.RunAsync();