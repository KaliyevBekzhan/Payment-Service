using Application.Dto;
using Application.Dto.Returns;
using Application.Repositories;
using Application.UseCases.Interfaces;
using FluentResults;

namespace Application.UseCases;

public class MyCabinetUseCase : IMyCabinetUseCase
{
    private readonly IGetMyPaymentsUseCase _getMyPaymentsUseCase;
    private readonly IUserRepository _userRepository;
    
    public MyCabinetUseCase(IUserRepository userRepository, IGetMyPaymentsUseCase getMyPaymentsUseCase)
    {
        _userRepository = userRepository;
        _getMyPaymentsUseCase = getMyPaymentsUseCase;
    }


    public async Task<Result<MyCabinetDto>> ExecuteAsync(int userId)
    {
        var userResult = await _userRepository.GetUserByIdAsync(userId);

        if (userResult.IsFailed)
        {
            return Result.Fail(userResult.Errors);
        }
        
        var transactionsResult = await _userRepository.GetMyActionsAsync(userId);
        
        if (transactionsResult.IsFailed)
        {
            return Result.Fail(transactionsResult.Errors);
        }
        
        var user = userResult.Value;
        
        var actions = transactionsResult.Value.Select(t => new ActionsDto(
            t.TransId,
            t.TransType == "payment" ? (t.OriginalAmount) * -1 : (t.OriginalAmount), 
            t.CurrencyName, 
            t.TransType == "payment" ? (t.AmountInTenge) * -1 : t.AmountInTenge, 
            t.StatusName, 
            t.Comment, 
            t.TransType,
            t.CreatedAt
        ));
        
        var result = new MyCabinetDto(user.Name, user.Account, user.WalletNumber, actions);
        
        return Result.Ok(result);
    }
}