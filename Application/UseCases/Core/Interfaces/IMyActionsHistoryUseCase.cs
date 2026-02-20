using Application.Dto;
using Application.Dto.Returns;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IMyActionsHistoryUseCase
{
    Task<Result<IEnumerable<ActionsDto>>> ExecuteAsync(GetMyActionsDto dto);
}