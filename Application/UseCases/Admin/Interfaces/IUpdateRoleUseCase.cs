using Application.Dto;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IUpdateRoleUseCase
{
    Task<Result> ExecuteAsync(UpdateRoleDto dto, int userId);
}