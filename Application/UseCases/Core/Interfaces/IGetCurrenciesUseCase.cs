using Application.Dto;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IGetCurrenciesUseCase
{
    Task<Result<IEnumerable<CurrenciesForUserDto>>> ExecuteAsync();
}