using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace API;

public static class Endpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/token", (LoginRequest request) =>
        {
            if (request.Password != "admin")
            {
                return Results.Unauthorized();
            }

            // Genera un JWT dummy
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Constants.SECRET_KEY);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // https://learn.microsoft.com/en-us/dotnet/api/System.Security.Claims.ClaimsIdentity?view=net-8.0
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, request.UserName),
                    new Claim(ClaimTypes.Email, $"{request.UserName}@localhost"),
                    new Claim(ClaimTypes.Role, "Administrator"),
                    new Claim(ClaimTypes.Role, "OtherRole")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = Constants.ISSUER
            };
  
            var token = tokenHandler.CreateToken(tokenDescriptor);
  
            return Results.Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        });
  
        app.MapGet("/api/claims", (HttpContext http) =>
        {
            var claims = http.User.Claims.Select(c => new { c.Type, c.Value });
            return claims;
        }).RequireAuthorization();
  
        app.MapGet("/api/products", () =>
        {
            var products = new[]
            {
                new { Id = 1, Name = "Producto 1" },
                new { Id = 2, Name = "Producto 2" },
                new { Id = 3, Name = "Producto 3" },
                new { Id = 4, Name = "Producto 4" },
                new { Id = 5, Name = "Producto 5" },
            };
            return products;
        }).RequireAuthorization();
    }
}
public record LoginRequest(string UserName, string Password);