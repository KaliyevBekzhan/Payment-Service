using Application.Dto;
using Application.Dto.Returns;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IAddCurrencyUseCase
{
    Task<Result<CurrencyInfoDto>> ExecuteAsync(AddCurrencyDto dto, int userId);
}