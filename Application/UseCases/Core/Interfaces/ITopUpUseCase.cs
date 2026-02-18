using FluentResults;

namespace Application.UseCases.Interfaces;

public interface ITopUpUseCase
{
    Task<Result> ExecuteAsync(int userId, decimal amount);
}