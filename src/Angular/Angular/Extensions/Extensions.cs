using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Yarp.ReverseProxy.Transforms;

public static class Extensions
{
    /// <summary>
    /// Configura la autenticación en el BFF utilizando cookies
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddLocalAuthentication(this IServiceCollection services)
    {
        services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = ".AJWbffAngular";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

                // Se sobreescribe OnRedirectToAccessDenied porque el comportamiento default de autenticación por cookies es la redirección a una página predeterminada
                // (ejem /AccessDenied) y como no estamos usando Razor Pages o similar, no queremos una redirección, sino el error HTTP 403.
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = 403;
                    return Task.CompletedTask;
                };
            });

        return services;
    }

    /// <summary>
    /// Configura el reverse proxy en el BFF utilizando YARP
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IServiceCollection AddBffProxy(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddReverseProxy()
            .LoadFromConfig(config.GetSection("ReverseProxy"))
            .AddTransforms(builderContext =>
            {
                builderContext.AddRequestTransform(transformContext =>
                {
                    if (transformContext.HttpContext.User.Identity!.IsAuthenticated)
                    {
                        var accessTokenClaim = transformContext.HttpContext.User.Claims
                            .FirstOrDefault(q => q.Type == "Access_Token");

                        if (accessTokenClaim != null)
                        {
                            var accessToken = accessTokenClaim.Value;

                            transformContext.ProxyRequest.Headers.Authorization =
                                new AuthenticationHeaderValue("Bearer", accessToken);
                        }
                    }

                    return ValueTask.CompletedTask;
                });
            });

        return services;

    }
 
}