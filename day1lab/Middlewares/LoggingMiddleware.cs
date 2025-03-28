namespace DemoAPI.Middlewares
{
    public class LoggingMiddleware
    {
        RequestDelegate next;
        ILogger<LoggingMiddleware> logger;
        public LoggingMiddleware(RequestDelegate _next,ILogger<LoggingMiddleware> _logger)
        {

            next = _next;
            logger = _logger;
        }
        public async Task Invoke(HttpContext context)
        {
            Console.WriteLine($"Request : {context.Request.Path} with {context.Request.Method}");
            logger.LogWarning($"Request : {context.Request.Path} with {context.Request.Method}");
            await next(context);
            Console.WriteLine($"Response : {context.Response.StatusCode} ");

        }
    }
}
