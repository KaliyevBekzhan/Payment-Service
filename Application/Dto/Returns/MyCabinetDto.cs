namespace Application.Dto.Returns;

public record MyCabinetDto(string Name, decimal Account, string WalletNumber, IEnumerable<ActionsDto> Payments);