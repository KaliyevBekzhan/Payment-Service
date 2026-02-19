namespace Application.Dto.Returns;

public record ActionsDto(int id,
    decimal OriginalAmount, 
    string Currency, 
    decimal AmountInTenge, 
    string Status,
    string Comment,
    string TransType,
    DateTime CreatedAt);