using Application.Dto.Returns;
using Application.Repositories;
using Application.UseCases.Interfaces;
using FluentResults;

namespace Application.UseCases.Admin;

public class GetUsersUseCase : IGetUsersUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IGuard _guard;
    public GetUsersUseCase(IUserRepository userRepository, IGuard guard)
    {
        _userRepository = userRepository;
        _guard = guard;
    }
    
    public async Task<Result<IEnumerable<AdminUsersDto>>> ExecuteAsync(int userId)
    {
        var validationResult = await _guard.ValidateAdminRole(userId, _userRepository);
        
        if (validationResult.IsFailed) return validationResult;
        
        var users = await _userRepository.GetAllUsersAsync();
        
        var result = users.Value.Select(u => new AdminUsersDto(
            u.Id, u.Name, u.IIN, u.Role.Name
        ));
        
        return Result.Ok(result);
    }
}