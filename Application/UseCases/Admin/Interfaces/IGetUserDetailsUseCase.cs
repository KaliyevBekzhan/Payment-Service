using Application.Dto.Returns;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IGetUserDetailsUseCase
{
    Task<Result<UserDetailsDto>> ExecuteAsync(int userId, int adminId);
}