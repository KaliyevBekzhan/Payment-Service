using Domain.Entity;
using FluentResults;

namespace Application.Repositories;

public interface IUserRepository
{
    Task<Result<User>> AddUserAsync(User user);
    Task<Result<User>> GetUserByIdAsync(int id);
    Task<Result> UpdateUserAccount(int id, decimal amount);
    Task<Result> UpdateUserCredentials(int id, string IIN, string password);
    Task<Result> UpdateUserName(int id, string name);
    Task<Result<User>> GetUserByIinAsync(string iin);
}