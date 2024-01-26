using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Angular.Endpoints;

public static class Endpoints
{
    public const string LocalLogin = "/local-login";
    public const string LocalLogout = "/local-logout";


    public static void MapEndpoints(this IEndpointRouteBuilder routes, IConfiguration config)
    {
        // Permite a los usuarios iniciar sesión enviando una solicitud POST con las credenciales de inicio de sesión.
        // El BFF envía las credenciales al backend para obtener un token de acceso (JWT).
        // Si la respuesta es exitosa, se crea una identidad de usuario con el token de acceso y
        // se inicia sesión utilizando el esquema de autenticación de cookies.
        // Además, se extraen algunos datos del JWT y se devuelven en la respuesta.
        routes.MapPost("/local-login", async (
            LoginRequest request,
            HttpContext httpContext,
            IHttpClientFactory httpClientFactory) =>
        {
            var client = httpClientFactory.CreateClient();
            var baseAddress = config["ApiHost:Url"];
            var response = await client.PostAsJsonAsync($"{baseAddress}/api/token", request);

            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

                var claims = new List<Claim>
                {
                    new Claim("Access_Token", loginResponse!.Token)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await httpContext.SignInAsync(claimsPrincipal);

                // Leer el token y obtener los claims utilizando JWT
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(loginResponse.Token);

                return Results.Ok(new
                {
                    token.ValidTo,
                    Name = token.Claims.Where(q => q.Type == "unique_name").FirstOrDefault()?.Value,
                    Roles = token.Claims.Where(q => q.Type == "role").Select(q => q.Value)
                });
            }

            return Results.Forbid();
        });

        // Permite a los usuarios cerrar sesión y se encarga de cerrar la sesión actual del usuario,
        // lo que ocasiona borrar las cookies de autenticación.
        routes.MapPost("/local-logout", async (HttpContext httpContext) =>
        {
            await httpContext.SignOutAsync();
            return Results.Ok();
        });
    }
}

public record LoginRequest(string UserName, string Password);

public record LoginResponse(string Token);