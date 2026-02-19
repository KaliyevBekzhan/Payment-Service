namespace PaymentServiceApi.Dto;

public record CreatePaymentRequest(decimal OriginalAmount, int CurrencyId, string Comment);