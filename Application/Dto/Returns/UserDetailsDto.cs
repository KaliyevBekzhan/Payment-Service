namespace Application.Dto.Returns;

public record UserDetailsDto(int Id, string Name, string Role, string WalletNumber, decimal Account, string Iin);