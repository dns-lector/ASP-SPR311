using ASP_SPR311.Data;
using ASP_SPR311.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;

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
                // context.Items.Add("auth", "OK");

                // У сесії - лише ID, знаходимо усі дані про користувача
                if (dataContext
                    .UserAccesses
                        .Include(ua => ua.UserData)  // команди на заповнення 
                        .Include(ua => ua.UserRole)  // навігаційних властивостей
                    .FirstOrDefault(ua =>
                        ua.Id.ToString() == context.Session.GetString("userAccessId"))
                    is UserAccess userAccess)
                {
                    // Claims - дані, що передаються до контексту за єдиною схемою:
                    //   пари ключ-значення, призначені для задач авторизації
                    context.User = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new Claim[]
                            {
                            new Claim( ClaimTypes.Sid,         userAccess.Id.ToString()  ),
                            new Claim( ClaimTypes.Name,        userAccess.UserData.Name  ),
                            new Claim( ClaimTypes.Email,       userAccess.UserData.Email ),
                            new Claim( ClaimTypes.MobilePhone, userAccess.UserData.Phone ),
                            new Claim( ClaimTypes.Role,        userAccess.UserRole.Id    ),
                            new Claim( "CanCreate", userAccess.UserRole.CanCreate.ToString() ),
                            new Claim( "CanRead",   userAccess.UserRole.CanRead.ToString()   ),
                            new Claim( "CanUpdate", userAccess.UserRole.CanUpdate.ToString() ),
                            new Claim( "CanDelete", userAccess.UserRole.CanDelete.ToString() ),
                            },
                            nameof(AuthSessionMiddleware)
                        )
                    );
                }                
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
