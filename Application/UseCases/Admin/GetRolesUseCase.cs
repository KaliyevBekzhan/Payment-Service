using Application.Dto;
using Application.Repositories;
using Application.UseCases.Interfaces;
using Domain.Entity;
using FluentResults;

namespace Application.UseCases.Admin;

public class GetRolesUseCase : IGetRolesUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IGuard _guard;
    private readonly IBaseRepository<Role> _roleRepository;
    public GetRolesUseCase(IUserRepository userRepository, IGuard guard, IBaseRepository<Role> roleRepository)
    {
        _userRepository = userRepository;
        _guard = guard;
        _roleRepository = roleRepository;
    }
    
    public async Task<Result<IEnumerable<RolesDto>>> ExecuteAsync(int userId)
    {
        var validationResult = await _guard.ValidateAdminRole(userId, _userRepository);
        
        if (validationResult.IsFailed) return validationResult;
        
        var roles = await _roleRepository.GetAllAsync();
        
        if (roles.IsFailed) return Result.Fail(roles.Errors);
        
        var result = roles.Value.Select(r => new RolesDto(r.Id, r.Name, r.IsAdmin, r.Priority));
        
        return Result.Ok(result);
    }
}