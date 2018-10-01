using Microsoft.AspNetCore.Builder;
using Devon4Net.Infrastructure.Middleware.Headers;

namespace Devon4Net.Infrastructure.Middleware.Configuration
{
    public static class MiddlewareConfiguration
    {
        public static IApplicationBuilder UseCustomHeadersMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomHeadersMiddleware>();
        }
    }
}
