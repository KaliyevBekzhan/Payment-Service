using Application.Dto;
using Application.Repositories;
using Application.UseCases.Interfaces;
using Domain.Entity;
using FluentResults;

namespace Application.UseCases.Admin;

public class UpdateRoleUseCase : IUpdateRoleUseCase
{
    private readonly IBaseRepository<Role> _roleRepository;
    private readonly IUserRepository _userRepository;   
    private readonly IGuard _guard;
    
    public UpdateRoleUseCase(IBaseRepository<Role> roleRepository, IUserRepository userRepository, IGuard guard)
    {
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _guard = guard;
    }
    
    public async Task<Result> ExecuteAsync(UpdateRoleDto dto, int userId)
    {
        var validationResult = await _guard.ValidateAdminRole(userId, _userRepository);
        
        if (validationResult.IsFailed) return validationResult;
        
        var result = await _roleRepository.UpdateAsync(new Role
        {
            Id = dto.Id, 
            Name = dto.Name,
            Priority = dto.Priority
        });
        
        return result;
    }
}