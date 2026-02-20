namespace Application.Dto;

public record GetMyActionsDto(int UserId, int PageNumber = 1, int PageSize = 10);