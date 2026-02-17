using Application.Dto.Returns;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IGetUsersUseCase
{
    Task<Result<IEnumerable<AdminUsersDto>>> ExecuteAsync(int userId);
}