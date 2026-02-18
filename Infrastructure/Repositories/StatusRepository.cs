using Application.Repositories;
using Domain.Entity;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class StatusRepository : IBaseRepository<Status>
{
    private readonly AppDbContext _dbContext;
    public StatusRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Result<Status>> GetByIdAsync(int id)
    {
        var result = await _dbContext.Statuses
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);
        
        if (result == null)
        {
            return Result.Fail("Status not found");
        }
        
        return Result.Ok(result);
    }

    public async Task<Result<IEnumerable<Status>>> GetAllAsync()
    {
        var result = await _dbContext.Statuses.AsNoTracking().ToListAsync();
        
        if (result.Count == 0)
        {
            return Result.Fail("No statuses found");
        }
        
        return Result.Ok(result.AsEnumerable());
    }

    public async Task<Result<Status>> AddAsync(Status entity)
    {
        _dbContext.Statuses.Add(entity);
        
        var rows = await _dbContext.SaveChangesAsync();
        
        if (rows == 0)
        {
            return Result.Fail("Status already exists");
        }
        
        return Result.Ok();
    }

    public async Task<Result> UpdateAsync(Status entity)
    {
        var rows = await _dbContext.Statuses
            .Where(s => s.Id == entity.Id)
            .ExecuteUpdateAsync(rs => 
                rs.SetProperty(s => s.Name, entity.Name));
        
        if (rows == 0)
        {
            return Result.Fail("No statuses was affected");
        }
        
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var rows = await _dbContext.Statuses
            .Where(s => s.Id == id)
            .ExecuteDeleteAsync();
        
        if (rows == 0)
        {
            return Result.Fail("No statuses was affected");
        }
        
        return Result.Ok();
    }
}