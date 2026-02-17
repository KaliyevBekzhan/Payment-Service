using Application.Dto;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IUpdateUserRoleUseCase
{
    Task<Result> Execute(UpdateUserRoleDto dto, int adminId);
}