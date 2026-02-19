using Application.Dto;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IAddTopupUseCase
{
    Task<Result> ExecuteAsync(AddTopupDto dto);
}