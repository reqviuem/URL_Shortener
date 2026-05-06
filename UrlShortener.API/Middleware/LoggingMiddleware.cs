using System.Diagnostics;

namespace UrlShortener.API.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation("Incoming Request: {Method} {Path}", 
            context.Request.Method, 
            context.Request.Path);

        var watch = Stopwatch.StartNew();
        
        await _next(context);

        watch.Stop();
        
        _logger.LogInformation("Outgoing Response: {StatusCode} for {Method} {Path} executed in {ElapsedMilliseconds} ms", 
            context.Response.StatusCode, 
            context.Request.Method, 
            context.Request.Path, 
            watch.ElapsedMilliseconds);
    }
}