using Application.Dto;
using Application.Dto.Returns;
using Domain.Entity;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IRegisterUserUseCase
{
    Task<Result<UserDto>> ExecuteAsync(RegisterUserDto userDto);
}