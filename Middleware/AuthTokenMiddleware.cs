using ASP_SPR311.Data;
using ASP_SPR311.Data.Entities;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Security.Claims;

namespace ASP_SPR311.Middleware
{
    public class AuthTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, DataContext dataContext)
        {
            String authHeader = context.Request.Headers.Authorization.ToString();
            String? errorMessage = null;
            String scheme = "Bearer ";
            Guid jti = default;
            if (String.IsNullOrEmpty(authHeader))
            {
                errorMessage = "Authorization header required";
            }
            else if (!authHeader.StartsWith(scheme))
            {
                errorMessage = $"Authorization scheme must be {scheme}";
            }
            else
            {
                String credentials = authHeader[scheme.Length..];
                try { jti = Guid.Parse(credentials); }
                catch
                {
                    errorMessage = "Authorization credentials invalid formatted";
                }
            }

            if (errorMessage == null)
            {
                var accessToken = dataContext
                    .AccessTokens
                    .Include(t => t.UserAccess)
                    .ThenInclude(ua => ua.UserRole)
                    .Include(t => t.User)
                    .FirstOrDefault(t => t.Jti == jti);

                if (accessToken == null)
                {
                    errorMessage = "Bearer credentials rejected";
                }
                else if (accessToken.Exp < DateTime.Now)
                {
                    errorMessage = "Bearer credentials expired";
                }
                else
                {
                    var userAccess = accessToken.UserAccess;
                    context.User = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            [
                            new Claim( ClaimTypes.Sid,         userAccess.Id.ToString()  ),
                            new Claim( ClaimTypes.Name,        accessToken.User.Name  ),
                            new Claim( ClaimTypes.Email,       accessToken.User.Email ),
                            new Claim( ClaimTypes.MobilePhone, accessToken.User.Phone ),
                            new Claim( ClaimTypes.Role,        userAccess.UserRole.Id    ),
                            new Claim( "CanCreate", userAccess.UserRole.CanCreate.ToString() ),
                            new Claim( "CanRead",   userAccess.UserRole.CanRead.ToString()   ),
                            new Claim( "CanUpdate", userAccess.UserRole.CanUpdate.ToString() ),
                            new Claim( "CanDelete", userAccess.UserRole.CanDelete.ToString() ),
                            ],
                            nameof(AuthTokenMiddleware)
                        )
                    );
                    context.Items.Add("AccessToken", accessToken);
                    // ?? Якщо треба подовжувати термін дії токена, то це робимо тут
                }
            }
            context.Items.Add(nameof(AuthTokenMiddleware), errorMessage);

            await _next(context);
        }
    }

    public static class AuthTokenMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthToken(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthTokenMiddleware>();
        }
    }
}
