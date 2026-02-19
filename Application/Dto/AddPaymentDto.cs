namespace Application.Dto;

public record AddPaymentDto (int UserId, decimal OriginalAmount, int CurrencyId, string Comment);