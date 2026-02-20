using Application.Dto;
using Application.Dto.Returns;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IGetMyTopupsUseCase 
{
    Task<Result<IEnumerable<ActionsDto>>> ExecuteAsync(GetMyActionsDto dto);
}