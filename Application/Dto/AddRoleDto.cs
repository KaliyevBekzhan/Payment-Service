namespace Application.Dto;

public record AddRoleDto(string Name, bool IsAdmin, int Priority);