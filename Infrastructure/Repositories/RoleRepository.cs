using System.Text;
using Application.Repositories;
using Domain.Entity;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoleRepository : IBaseRepository<Role>
{
    private readonly AppDbContext _dbContext;
    public RoleRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Result<Role>> GetByIdAsync(int id)
    {
        var result = await _dbContext.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);
        
        if (result == null)
        {
            return Result.Fail("Role not found");
        }
        
        return result;
    }

    public async Task<Result<IEnumerable<Role>>> GetAllAsync()
    {
        var result = await _dbContext.Roles
            .AsNoTracking()
            .ToListAsync();

        if (result.Count == 0)
        {
            return Result.Fail("No roles found");
        }
        
        return Result.Ok(result.AsEnumerable());
    }

    public async Task<Result> AddAsync(Role entity)
    {
        _dbContext.Roles.Add(entity);

        var rows = await _dbContext.SaveChangesAsync();
        
        if (rows == 0)
        {
            return Result.Fail("Role already exists");
        }

        return Result.Ok();
    }

    public async Task<Result> UpdateAsync(Role entity)
    {
        var row = await _dbContext.Roles
            .Where(r => r.Id == entity.Id)
            .Where(r => r.IsAdmin == false)
            .ExecuteUpdateAsync(rs => rs
                .SetProperty(r => r.Name, entity.Name)
            );

        if (row == 0)
        {
            return Result.Fail("No roles was affected");
        }
        
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var row = await _dbContext.Roles
            .Where(r => r.Id == id)
            .Where(r => r.IsAdmin == false)
            .ExecuteDeleteAsync();
        
        if (row == 0)
        {
            return Result.Fail("No roles was affected");
        }
        
        return Result.Ok();
    }
}