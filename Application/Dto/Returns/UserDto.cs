namespace Application.Dto.Returns;

public record UserDto(string Role, 
    string Token, 
    DateTime Expires);
