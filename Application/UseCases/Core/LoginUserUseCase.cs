using Application.Dto;
using Application.Dto.Returns;
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
    private readonly IPasswordHasher _passwordHasher;
    
    public LoginUserUseCase(IJwtService jwtService, 
        IUserRepository userRepository,
        IGuard guard,
        IPasswordHasher passwordHasher)
    {
        _jwtService = jwtService;
        _userRepository = userRepository;
        _guard = guard;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<Result<TokenDto>> Execute(LoginDto dto)
    {
        var result = await _userRepository.GetUserByIinAsync(dto.iin);

        var validationResult = _guard.AuthUserValidate(result);
        if (validationResult.IsFailed)
        {
            return Result.Fail(validationResult.Errors);
        }
        
        var user = result.Value;
        
        if (!_passwordHasher.VerifyPassword(dto.password, user.Password))
        {
            return Result.Fail("Invalid password");
        }
        
        var token = _jwtService.GenerateToken(user.Id, user.Role.Name);
        
        return Result.Ok(new TokenDto(token.Token, token.Expires, user.Role.Name));
    }
}