using Application.Dto.Returns;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IGetCurrencyInfoUseCase
{
    Task<Result<CurrencyInfoDto>> ExecuteAsync(int currencyId, int userId);
}