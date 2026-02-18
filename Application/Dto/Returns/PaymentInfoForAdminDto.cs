namespace Application.Dto.Returns;

public record PaymentInfoForAdminDto(
    int Id,
    int UserId,
    string UserName,
    decimal OriginalAmount,
    string Currency,
    decimal AmountInTenge,
    decimal Account,
    DateTime CreatedAt,
    string WalletNumber,
    string Comment,
    string Status,
    int? ChangerId,
    string? ChangerName
    );