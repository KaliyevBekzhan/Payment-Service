using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IDeleteRoleUseCase
{
    Task<Result> ExecuteAsync(int id, int userId);
}