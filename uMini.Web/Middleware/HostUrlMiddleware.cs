namespace uMini.Web.Middleware;

public class HostUrlMiddleware
{
    private readonly RequestDelegate _next;

    public HostUrlMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        var urlScheme = httpContext.Request.Scheme;
        var urlHost = httpContext.Request.Host;
        var urlPathBase = httpContext.Request.PathBase;

        httpContext.Items.Add("HostUrl", $"{urlScheme}://{urlHost}{urlPathBase}/");
        await _next(httpContext);
    }
}

public static class HostUrlMiddlewareExtensions
{
    public static IApplicationBuilder UseHostUrlMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<HostUrlMiddleware>();
    }
}