using Application.Dto;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IAddCurrencyUseCase
{
    Task<Result> ExecuteAsync(AddCurrencyDto dto, int userId);
}