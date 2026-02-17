namespace Application.Dto.Returns;

public record AdminPaymentsDto(string UserName, 
    decimal OriginalAmount,
    decimal AmountInTenge,
    decimal Account,
    string WalletNumber,
    string Currency,
    int PaymentId,
    string Role,
    DateTime CreatedAt);