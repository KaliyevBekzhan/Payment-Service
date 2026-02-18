using Application.Dto;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IUpdateCurrencyUseCase
{
    Task<Result> ExecuteAsync(UpdateCurrencyDto dto, int userId);
}