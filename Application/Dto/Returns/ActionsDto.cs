namespace Application.Dto.Returns;

public record ActionsDto(decimal OriginalAmount, 
    string Currency, 
    decimal AmountInTenge, 
    string Status,
    string Comment,
    string TransType);