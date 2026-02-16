namespace Application.Dto.Security;

public record TokenClaims(int UserId, string Role);