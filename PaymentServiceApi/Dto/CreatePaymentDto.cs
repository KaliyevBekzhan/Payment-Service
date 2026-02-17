namespace PaymentServiceApi.Dto;

public record CreatePaymentDto(decimal Amount, int CurrencyId, string Comment);