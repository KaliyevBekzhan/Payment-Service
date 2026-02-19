namespace Application.Dto;

public record TopupInfoDto(int PaymentId, 
    decimal OriginalAmount, 
    string Currency, 
    string Status, 
    decimal AmountInTenge,
    decimal AccountAtTheTimeOfPayment,
    DateTime CreatedAt);