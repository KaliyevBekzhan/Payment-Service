using Application.Dto.Returns;
using Domain.Entity;
using FluentResults;

namespace Application.Repositories;

public interface IUserRepository
{
    Task<Result<User>> AddUserAsync(User user);
    Task<Result<User>> GetUserByIdAsync(int id);
    Task<Result> UpdateUserAccountAsync(int id, decimal amount);
    Task<Result> UpdateUserCredentialsAsync(int id, string IIN, string password);
    Task<Result> UpdateUserNameAsync(int id, string name);
    Task<Result> UpdateUserRoleAsync(int id, int roleId);
    Task<Result<User>> GetUserByIinAsync(string iin);
    Task<Result<bool>> CheckUserIsAdminAsync(int userId);
    Task<Result<IEnumerable<User>>> GetAllUsersAsync();
    Task<Result> UpdateUserAccountBalanceAsync(int id, decimal amount);
    Task<Result<IEnumerable<TransactionsView>>> GetMyActionsAsync(int userId);
}