namespace Application.Dto.Returns;

public record PaymentDto(int id,
    decimal OriginalAmount, 
    string Currency, 
    decimal AmountInTenge, 
    string Status,
    string Comment,
    DateTime CreatedAt);
