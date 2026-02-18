using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Dto.Security;
using Application.Repositories;
using FluentResults;
using Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;
    public JwtService(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }


    public (string Token, DateTime Expires) GenerateToken(int userId, string role)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
        
        var expires = DateTime.UtcNow.AddHours(2); 
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
               new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
               new Claim(ClaimTypes.Role, role)
            }),
            Expires = expires,
            Audience = _jwtSettings.Audience,
            Issuer = _jwtSettings.Issuer,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature
            )
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return (tokenHandler.WriteToken(token), expires);
    }

    public Result<TokenClaims> ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            if (validatedToken == null)
            {
                return Result.Fail("Invalid token");
            }

            int userId;
            string role = principal.FindFirst(ClaimTypes.Role)?.Value;

            if (principal.FindFirst(ClaimTypes.NameIdentifier) == null)
            {
                return Result.Fail("Invalid token");
            }
            else
            {
                userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier).Value);
            }

            var tokenClaims = new TokenClaims(userId, role);

            return Result.Ok(tokenClaims);
        }
        catch (Exception e)
        {
            return Result.Fail(e.Message);
        }
    }
}