using Application.Dto;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IGetRolesUseCase
{
    Task<Result<IEnumerable<RolesDto>>> ExecuteAsync(int userId);
}