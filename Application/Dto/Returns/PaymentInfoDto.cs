namespace Application.Dto.Returns;

public record PaymentInfoDto(int PaymentId, 
    decimal OriginalAmount, 
    string Currency, 
    string Status, 
    decimal AmountInTenge,
    decimal AccountAtTheTimeOfPayment,
    DateTime CreatedAt);