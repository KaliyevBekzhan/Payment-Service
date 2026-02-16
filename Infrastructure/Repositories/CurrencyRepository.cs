using Application.Repositories;
using Domain.Entity;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CurrencyRepository : IBaseRepository<Currency>
{
    private readonly AppDbContext _dbContext;
    public CurrencyRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Result<Currency>> GetByIdAsync(int id)
    {
        var result = await _dbContext.Currencies.FindAsync(id);
        
        if (result == null)
        {
            return Result.Fail("Currency not found");
        }
        
        return Result.Ok(result);
    }

    public async Task<Result<IEnumerable<Currency>>> GetAllAsync()
    {
        var result = await _dbContext.Currencies.AsNoTracking().ToListAsync();

        if (result.Count == 0)
        {
            return Result.Fail("No currencies found");
        }
        
        return Result.Ok(result.AsEnumerable());
    }

    public async Task<Result> AddAsync(Currency entity)
    {
        _dbContext.Currencies.Add(entity);
        
        var rows = await _dbContext.SaveChangesAsync();

        if (rows == 0)
        {
            return Result.Fail("Currency already exists");
        }
        
        return Result.Ok();
    }

    public async Task<Result> UpdateAsync(Currency entity)
    {
        var rows = await _dbContext.Currencies
            .Where(c => c.Id == entity.Id)
            .ExecuteUpdateAsync(rs => 
                rs.SetProperty(c => c.Name, entity.Name));
        
        if (rows == 0)
        {
            return Result.Fail("No currencies was affected");
        }
        
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(Currency entity)
    {
        var rows = await _dbContext.Currencies
            .Where(c => c.Id == entity.Id)
            .ExecuteDeleteAsync();

        if (rows == 0)
        {
            return Result.Fail("No currencies was affected");
        }
        
        return Result.Ok();
    }
}