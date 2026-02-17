using Application.Repositories;
using Application.UseCases.Interfaces;
using Domain.Entity;
using FluentResults;

namespace Application.UseCases.Admin;

public class DeleteRoleUseCase : IDeleteRoleUseCase
{
    private readonly IBaseRepository<Role> _roleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGuard _guard;

    public DeleteRoleUseCase(
        IBaseRepository<Role> roleRepository,
        IUserRepository userRepository,
        IGuard guard
        )
    {
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _guard = guard;
    }
    
    public async Task<Result> ExecuteAsync(int id, int userId)
    {
        var validationResult = await _guard.ValidateAdminRole(userId, _userRepository);
        
        if (validationResult.IsFailed) return validationResult;
        
        var result = await _roleRepository.DeleteAsync(id);
        
        return result;
    }
}