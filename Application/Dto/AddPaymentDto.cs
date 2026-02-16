namespace Application.Dto;

public record AddPaymentDto (int UserId, decimal Amount, int CurrencyId, string Comment);