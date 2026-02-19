using Application.Dto.Returns;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IGetMyTopupsUseCase
{
    Task<Result<IEnumerable<MyTopupsDto>>> ExecuteAsync(int userId);
}