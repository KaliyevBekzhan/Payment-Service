using Application.Dto.Returns;
using Application.Repositories;
using Application.UseCases.Interfaces;
using FluentResults;

namespace Application.UseCases.Admin;

public class GetUserDetailsUseCase : IGetUserDetailsUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IGuard _guard;
    public GetUserDetailsUseCase(IUserRepository userRepository, IGuard guard)
    {
        _userRepository = userRepository;
        _guard = guard;
    }


    public async Task<Result<UserDetailsDto>> ExecuteAsync(int userId, int adminId)
    {
        var validationResult = await _guard.ValidateAdminRole(adminId, _userRepository);
        
        if (validationResult.IsFailed) return validationResult;
        
        var userResult = await _userRepository.GetUserByIdAsync(userId);
        if (userResult == null)
        {
            return Result.Fail("User not found");
        }
        
        var user = userResult.Value;
        
        return Result.Ok(new UserDetailsDto(
            user.Id, user.Name, user.Role.Name, user.WalletNumber, user.Account, user.IIN
        ));
    }
}