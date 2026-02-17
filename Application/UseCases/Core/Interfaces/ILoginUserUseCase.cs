using Application.Dto;
using Application.Dto.Returns;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface ILoginUserUseCase
{
    Task<Result<TokenDto>> Execute(LoginDto dto);
}