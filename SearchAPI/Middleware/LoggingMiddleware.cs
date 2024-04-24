namespace SearchAPI.Middleware
{
    using Microsoft.Extensions.Logging;
    using Serilog;

    public class LoggingMiddleware
     {
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
            try
            {

                string url = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";

                Log.Information("Requested URL: {Url}", url);
                var originalBodyStream = context.Response.Body;
                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;

                    // Call the next middleware in the pipeline
                    await _next(context);

                    // Read the response body from the MemoryStream
                    responseBody.Seek(0, SeekOrigin.Begin);
                    string responseBodyContent = await new StreamReader(responseBody).ReadToEndAsync();

                    // Log the response body
                    Log.Information($"Response body: {responseBodyContent}");

                    // Copy the response body back to the original stream
                    responseBody.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(originalBodyStream);

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unhandled exception occurred");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");

            }
    }
}
}
