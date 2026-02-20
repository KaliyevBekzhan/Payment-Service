using Application.Dto;
using Application.Dto.Returns;
using Application.Repositories;
using Application.UseCases.Interfaces;
using FluentResults;

namespace Application.UseCases;

public class MyActionsHistoryUseCase : IMyActionsHistoryUseCase
{
    private readonly IUserRepository _userRepository;
    
    public MyActionsHistoryUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<IEnumerable<ActionsDto>>> ExecuteAsync(GetMyActionsDto dto)
    {
        var actionsResult = await _userRepository.GetMyActionsAsync(dto.UserId, 
            dto.PageNumber, dto.PageSize);
        
        if (actionsResult.IsFailed) return Result.Fail(actionsResult.Errors);
        
        return Result.Ok(actionsResult.Value.Select(action => new ActionsDto(
            action.TransId,
            action.TransType == "payment" ? (action.OriginalAmount) * -1 : (action.OriginalAmount),
            action.CurrencyName,
            action.TransType == "payment" ? (action.AmountInTenge) * -1 : action.AmountInTenge,
            action.StatusName,
            null,
            action.Comment,
            action.TransType,
            action.CreatedAt
        )));
    }
}