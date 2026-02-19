namespace Application.Dto.Returns;

public record MyTopupsDto(decimal OriginalAmount, decimal AmountInTenge, int CurrencyId, string Comment, DateTime CreatedAt);