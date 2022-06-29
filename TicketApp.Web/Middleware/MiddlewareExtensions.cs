using Microsoft.AspNetCore.Builder;

namespace TicketApp.Web.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddleware<T>(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<T>();
        }
    }
}