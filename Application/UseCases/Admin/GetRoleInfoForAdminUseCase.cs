using Application.Dto;
using Application.Repositories;
using Application.UseCases.Interfaces;
using Domain.Entity;
using FluentResults;

namespace Application.UseCases.Admin;

public class GetRoleInfoForAdminUseCase : IGetRoleInfoForAdminUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IGuard _guard;
    private readonly IBaseRepository<Role> _roleRepository;
    
    public GetRoleInfoForAdminUseCase(IUserRepository userRepository, IGuard guard, IBaseRepository<Role> roleRepository)
    {
        _userRepository = userRepository;
        _guard = guard;
        _roleRepository = roleRepository;
    }
    
    public async Task<Result<RoleInfoDto>> ExecuteAsync(int roleId, int userId)
    {
        var validationResult = await _guard.ValidateAdminRole(userId, _userRepository);

        if (validationResult.IsFailed)
        {
            return Result.Fail(validationResult.Errors);
        }
        
        var roleResult = await _roleRepository.GetByIdAsync(roleId);
        
        if (roleResult.IsFailed)
        {
            return roleResult.ToResult<RoleInfoDto>();
        }
        
        return Result.Ok(new RoleInfoDto(roleResult.Value.Id, roleResult.Value.Name, roleResult.Value.Priority));
    }
}