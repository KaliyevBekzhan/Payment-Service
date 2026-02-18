using Application.Repositories;
using Application.UseCases.Interfaces;
using Domain.Policies;
using FluentResults;

namespace Application.UseCases;

public class TopUpUseCase : ITopUpUseCase
{
    private readonly IUserRepository _userRepository;
    
    public TopUpUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }


    public async Task<Result> ExecuteAsync(int userId, decimal amount)
    {
        var userResult = await _userRepository.GetUserByIdAsync(userId);

        if (userResult.IsFailed)
        {
            return Result.Fail(userResult.Errors);
        }
        
        var result = TopUpPolicy.ValidateTopUp(userResult.Value, amount);

        if (result.IsFailed)
        {
            return Result.Fail(result.Errors);
        }
        
        await _userRepository.UpdateUserAccountBalanceAsync(userResult.Value.Id, result.Value);
        
        return Result.Ok();
    }
}