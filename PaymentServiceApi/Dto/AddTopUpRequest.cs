namespace PaymentServiceApi.Dto;

public record AddTopUpRequest(decimal OriginalAmount, int CurrencyId, string Comment);