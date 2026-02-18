using Application.Dto;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IGetRoleInfoForAdminUseCase
{
    Task<Result<RoleInfoDto>> ExecuteAsync(int roleId, int userId);
}