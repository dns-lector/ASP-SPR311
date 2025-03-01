using ASP_SPR311.Data;
using System.Globalization;

namespace ASP_SPR311.Middleware
{
    public class AuthSessionMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthSessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // !! Інжекція до Middleware здійснюється через метод (конструктор "зайнятий")
        public async Task InvokeAsync(HttpContext context, DataContext dataContext)
        {
            if (context.Request.Query.ContainsKey("logout"))
            {
                context.Session.Remove("userAccessId");
                context.Response.Redirect(context.Request.Path);
                return;
            }
            if (context.Session.Keys.Contains("userAccessId"))
            {
                // Користувач автентифікований
                context.Items.Add("auth", "OK");

                // У сесії - лише ID, знаходимо усі дані про користувача
                // dataContext.UserAccesses
            }
            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
    }


    public static class AuthSessionMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthSession(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthSessionMiddleware>();
        }
    }
}
