namespace Application.Dto.Returns;

public record TokenDto(string Token, DateTime Expires, string Role);