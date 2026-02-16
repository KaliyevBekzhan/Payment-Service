using Application.Repositories;
using Domain.Entity;
using FluentResults;
using Microsoft.EntityFrameworkCore;

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

    public async Task<Result> UpdateUserAccount(int id, decimal account)
    {
        var result = await GetUserByIdAsync(id);
        
        if (result.IsSuccess == true)
        {
            result.Value.Account = account;
            return Result.Ok();
        }
        
        return Result.Fail(result.Errors);
    }

    public async Task<Result> UpdateUserCredentials(int id, string IIN, string password)
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

    public async Task<Result> UpdateUserName(int id, string name)
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
}