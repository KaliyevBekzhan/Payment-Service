namespace PaymentServiceApi.Dto;

public record CreatePaymentRequest(decimal Amount, int CurrencyId, string Comment);