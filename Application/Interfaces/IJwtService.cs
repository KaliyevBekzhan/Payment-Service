using Application.Dto.Security;
using FluentResults;

namespace Application.Repositories;

public interface IJwtService
{
    string GenerateToken(int userId, string role);
    Result<TokenClaims> ValidateToken(string token);
}