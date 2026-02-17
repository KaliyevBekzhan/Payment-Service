using Application.Dto;
using Application.Repositories;
using Application.UseCases.Interfaces;
using FluentResults;

namespace Application.UseCases.Admin;

public class UpdateUserRoleUseCase : IUpdateUserRoleUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IGuard _guard;
    
    public UpdateUserRoleUseCase(IUserRepository userRepository, IGuard guard)
    {
        _userRepository = userRepository;
        _guard = guard;
    }


    public async Task<Result> Execute(UpdateUserRoleDto dto, int adminId)
    {
        var validationResult = await _guard.ValidateAdminRole(adminId, _userRepository);
        
        if (validationResult.IsFailed) return validationResult;
        
        var updateResult = await _userRepository.UpdateUserRoleAsync(dto.UserId, dto.RoleId);
        
        return updateResult;
    }
}