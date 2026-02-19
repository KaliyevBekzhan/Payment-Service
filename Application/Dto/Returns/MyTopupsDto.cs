namespace Application.Dto.Returns;

public record MyTopupsDto(int id, decimal OriginalAmount, decimal AmountInTenge, string Currency, string Comment, string Status, DateTime CreatedAt);