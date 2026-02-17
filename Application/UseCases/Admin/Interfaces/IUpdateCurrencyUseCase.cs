using Application.Dto;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IUpdateCurrencyUseCase
{
    Task<Result> UpdateCurrencyAsync(UpdateCurrencyDto dto, int userId);
}