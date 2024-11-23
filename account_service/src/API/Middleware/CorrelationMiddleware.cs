using System.Diagnostics;
using Serilog;
using Serilog.Context;

namespace API.Middleware;

public class CorrelationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Skip middleware for Swagger or base path
        if (context.Request.Path.StartsWithSegments("/swagger", StringComparison.OrdinalIgnoreCase) ||
            context.Request.Path == "/" ||
            string.IsNullOrEmpty(context.Request.Path.Value))
        {
            await next(context);
            return;
        }

        var stopwatch = Stopwatch.StartNew(); // Start timing the request

        // Generate or retrieve the correlation ID
        var correlationId = context.Request.Headers["x-correlation-id"].FirstOrDefault() ?? Guid.NewGuid().ToString();
        context.Items["CorrelationId"] = correlationId;
        LogContext.PushProperty("CorrelationId", correlationId);

        // Construct the request URI
        var requestUri = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";

        try
        {
            Log.Information("Received {Method} request for {RequestUri}", context.Request.Method, requestUri);

            await next(context); // Process the next middleware
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled exception occurred.");
            throw; // Rethrow to ensure the exception propagates properly
        }
        finally
        {
            // Stop timing and log the response time
            stopwatch.Stop();
            var responseTime = stopwatch.ElapsedMilliseconds;

            Log.Information("Processed {Method} request for {RequestUri} in {ResponseTime} ms",
                context.Request.Method, requestUri, responseTime);
        }
    }
}