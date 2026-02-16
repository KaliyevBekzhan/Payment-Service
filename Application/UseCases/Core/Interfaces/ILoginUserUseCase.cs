using FluentResults;

namespace Application.UseCases.Interfaces;

public interface ILoginUserUseCase
{
    Task<Result<string>> Execute(string iin, string password);
}