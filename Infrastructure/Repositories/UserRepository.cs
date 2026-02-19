using Application.Dto.Returns;
using Application.Repositories;
using Domain.Entity;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;
    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Result<User>> AddUserAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        
        var row = await _dbContext.SaveChangesAsync();

        if (row == 0)
        {
            return Result.Fail("User already exists");
        }
        
        return Result.Ok(user);
    }

    public async Task<Result<User>> GetUserByIdAsync(int id)
    {
        var result = await _dbContext.Users.FindAsync(id);

        if (result == null)
        {
            return Result.Fail("User not found");
        }
        
        return Result.Ok(result);
    }

    public async Task<Result> UpdateUserAccountAsync(int id, decimal account)
    {
        var result = await GetUserByIdAsync(id);
        
        if (result.IsSuccess == true)
        {
            result.Value.Account = account;
            return Result.Ok();
        }
        
        return Result.Fail(result.Errors);
    }

    public async Task<Result> UpdateUserCredentialsAsync(int id, string IIN, string password)
    {
        var rows = await _dbContext.Users
            .Where(u => u.Id == id)
            .ExecuteUpdateAsync(rs => rs
                .SetProperty(u => u.IIN, IIN)
                .SetProperty(u => u.Password, password)
            );

        if (rows == 0)
        {
            return Result.Fail("No users was affected");
        }
        
        return Result.Ok();
    }

    public async Task<Result> UpdateUserNameAsync(int id, string name)
    {
        var rows = await _dbContext.Users
            .Where(u => u.Id == id)
            .ExecuteUpdateAsync(rs => rs.SetProperty(u => u.Name, name));
        
        if (rows == 0)
        {
            return Result.Fail("No users was affected");
        }
        
        return Result.Ok();
    }

    public async Task<Result> UpdateUserRoleAsync(int id, int roleId)
    {
        var rows = await _dbContext.Users
            .Where(u => u.Id == id)
            .ExecuteUpdateAsync(rs => rs.SetProperty(u => u.RoleId, roleId));
        
        if (rows == 0)
        {
            return Result.Fail("No users was affected");
        }
        
        return Result.Ok();
    }

    public async Task<Result<User>> GetUserByIinAsync(string iin)
    {
        var result = await _dbContext.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.IIN == iin);

        if (result == null)
        {
            return Result.Fail("User not found");
        }
        
        return Result.Ok(result);
    }

    public async Task<Result<bool>> CheckUserIsAdminAsync(int userId)
    {
        var result = await _dbContext.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (result == null)
        {
            return Result.Fail("User not found");
        }
        
        return Result.Ok(result.Role.IsAdmin);
    }

    public async Task<Result<IEnumerable<User>>> GetAllUsersAsync()
    {
        var result = await _dbContext.Users
            .Include(u => u.Role)
            .ToListAsync();
        
        return Result.Ok(result.AsEnumerable());
    }

    public async Task<Result> UpdateUserAccountBalanceAsync(int id, decimal balance)
    {
        var rows = await _dbContext.Users
            .Where(u => u.Id == id)
            .ExecuteUpdateAsync(rs => rs.SetProperty(u =>
                u.Account, balance
            ));

        if (rows == 0)
        {
            return Result.Fail("No User was affected");
        }
        
        return Result.Ok();
    }

    public async Task<Result<IEnumerable<TransactionsView>>> GetMyActionsAsync(int userId)
    {
        var result = await _dbContext.Transactions
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
        
        return Result.Ok(result.AsEnumerable());
    }
}