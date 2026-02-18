namespace Application.Dto.Returns;

public record PaymentDto(decimal OriginalAmount, 
    string Currency, 
    decimal AmountInTenge, 
    string Status,
    string Comment);
