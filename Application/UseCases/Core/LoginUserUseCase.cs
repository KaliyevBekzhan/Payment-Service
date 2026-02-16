using Application.Dto.Security;
using Application.Repositories;
using Application.UseCases.Interfaces;
using Domain.Entity;
using FluentResults;

namespace Application.UseCases;

public class LoginUserUseCase : ILoginUserUseCase
{
    private readonly IJwtService _jwtService;
    private readonly IUserRepository _userRepository;
    private readonly IGuard _guard;
    public LoginUserUseCase(IJwtService jwtService, 
        IUserRepository userRepository,
        IGuard guard)
    {
        _jwtService = jwtService;
        _userRepository = userRepository;
        _guard = guard;
    }
    
    public async Task<Result<string>> Execute(string iin, string password)
    {
        var result = await _userRepository.GetUserByIinAsync(iin);

        var validationResult = _guard.AuthUserValidate(result);
        if (validationResult.IsFailed)
        {
            return Result.Fail(validationResult.Errors);
        }
        
        var user = result.Value;
        
        var token = _jwtService.GenerateToken(user.Id, user.Role.Name);
        
        return Result.Ok(token);
    }
}