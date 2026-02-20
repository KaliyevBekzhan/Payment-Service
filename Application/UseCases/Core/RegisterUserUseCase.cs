using Application.Dto;
using Application.Dto.Returns;
using Application.Repositories;
using Application.UseCases.Interfaces;
using Domain.Entity;
using Domain.Enums;
using FluentResults;

namespace Application.UseCases;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IGuard _guard;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IWalletNumberGenerator _walletNumberGenerator;
    private readonly IJwtService _jwtService;
    public RegisterUserUseCase(IUserRepository userRepository, 
        IGuard guard, 
        IPasswordHasher passwordHasher,
        IWalletNumberGenerator walletNumberGenerator,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _guard = guard;
        _passwordHasher = passwordHasher;
        _walletNumberGenerator = walletNumberGenerator;
        _jwtService = jwtService;
    }

    public async Task<Result<UserDto>> ExecuteAsync(RegisterUserDto userDto)
    {
        var existingUser = await _userRepository.GetUserByIinAsync(userDto.Iin);

        if (existingUser.IsSuccess)
        {
            return Result.Fail<UserDto>("User already exists");
        }

        var validationResult = _guard.RegisterUserValidate(userDto);
        if (validationResult.IsFailed)
        {
            return Result.Fail<UserDto>(validationResult.Errors);
        }
        
        User user = new User
        {
            IIN = userDto.Iin,
            Name = userDto.Name,
            Password = _passwordHasher.HashPassword(userDto.Password),
            RoleId = (int)RolesEnum.User,
            Account = 0,
            WalletNumber = _walletNumberGenerator.Generate(),
            Role = new Role
            {
                Id = (int)RolesEnum.User,
                Name = "User",
                IsAdmin = false,
                Priority = 1
            }
        };
        
        await _userRepository.AddUserAsync(user);
        
        var token = _jwtService.GenerateToken(user.Id, user.Role.Name);
        
        return Result.Ok<UserDto>(new UserDto(user.Id, user.Name, user.Role.Name, user.Account, user.WalletNumber, token.Token, token.Expires));
    }
}