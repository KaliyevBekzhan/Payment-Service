using Application.Dto.Security;
using FluentResults;

namespace Application.Repositories;

public interface IJwtService
{
    (string Token, DateTime Expires) GenerateToken(int userId, string role);
    Result<TokenClaims> ValidateToken(string token);
}