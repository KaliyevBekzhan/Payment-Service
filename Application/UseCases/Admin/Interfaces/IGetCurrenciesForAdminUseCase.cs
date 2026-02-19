using Application.Dto;
using Application.Dto.Returns;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IGetCurrenciesForAdminUseCase
{
    Task<Result<IEnumerable<CurrenciesDto>>> ExecuteAsync(int userId);
}