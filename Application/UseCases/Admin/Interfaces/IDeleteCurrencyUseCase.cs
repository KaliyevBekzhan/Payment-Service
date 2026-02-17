using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IDeleteCurrencyUseCase 
{
    Task<Result> ExecuteAsync(int id, int userId);
}