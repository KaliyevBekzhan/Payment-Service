using Application.Dto;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IAddRoleUseCase
{
    Task<Result> ExecuteAsync(AddRoleDto dto, int userId);
}