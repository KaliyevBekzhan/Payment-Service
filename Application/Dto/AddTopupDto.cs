namespace Application.Dto;

public record AddTopupDto(int UserId, decimal OriginalAmount, int CurrencyId, string Comment);