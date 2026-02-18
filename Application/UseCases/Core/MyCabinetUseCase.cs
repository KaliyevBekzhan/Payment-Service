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
        
        var paymentsResult = await _getMyPaymentsUseCase.ExecuteAsync(new GetMyPaymentsDto(userId));

        if (paymentsResult.IsFailed)
        {
            return Result.Fail(paymentsResult.Errors);
        }
        
        var user = userResult.Value;
        
        var result = new MyCabinetDto(user.Name, user.Account, user.WalletNumber, paymentsResult.Value);
        
        return Result.Ok(result);
    }
}