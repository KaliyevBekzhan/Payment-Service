using Application.Dto;
using Application.Repositories;
using Application.UseCases.Interfaces;
using Domain.Entity;
using FluentResults;

namespace Application.UseCases.Admin;

public class AddRoleUseCase : IAddRoleUseCase
{
    private readonly IBaseRepository<Role> _roleRepository;
    private readonly IUserRepository _userRepository;   
    private readonly IGuard _guard;
    
    public AddRoleUseCase(IBaseRepository<Role> roleRepository, IGuard guard, IUserRepository userRepository)
    {
        _roleRepository = roleRepository;
        _guard = guard;
        _userRepository = userRepository;
    }


    public async Task<Result> ExecuteAsync(AddRoleDto dto, int userId)
    {
        var validationResult = await _guard.ValidateAdminRole(userId, _userRepository);
        
        if (validationResult.IsFailed) return validationResult;
        
        var role = new Role
        {
            Name = dto.Name,
            IsAdmin = dto.IsAdmin,
            Priority = dto.Priority
        };
        
        var result = await _roleRepository.AddAsync(role);

        return result;
    }
}