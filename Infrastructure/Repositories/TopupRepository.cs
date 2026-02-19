using Application.Interfaces;
using Application.Repositories;
using Domain.Entity;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TopupRepository : ITopupRepository
{
    private readonly AppDbContext _dbContext;
    public TopupRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> AddTopupAsync(TopUp topUp)
    {
        await _dbContext.TopUps.AddAsync(topUp);
        
        var rows = await _dbContext.SaveChangesAsync();
        
        return rows > 0 ? Result.Ok() : Result.Fail("Topup failed");
    }

    public async Task<Result<IEnumerable<TopUp>>> GetTopupsAsync()
    {
        var result = await _dbContext.TopUps.ToListAsync();
        
        if (result.Count == 0) return Result.Fail("No topups found");
        
        return Result.Ok(result.AsEnumerable());
    }

    public async Task<Result<IEnumerable<TopUp>>> GetTopupByUserIdAsync(int userId)
    {
        var result = await _dbContext.TopUps
            .Where(t => t.UserId == userId)
            .ToListAsync();

        if (result.Count == 0) return Result.Fail("No topups found");

        return Result.Ok(result.AsEnumerable());
    }

    public async Task<Result<TopUp>> GetTopupByIdAsync(int id)
    {
        var result = await _dbContext.TopUps
            .Include(t => t.Status)
            .Include(t => t.Currency)
            .FirstOrDefaultAsync(t => t.Id == id);
        
        if (result == null)
        {
            return Result.Fail("Topup not found");
        }
        
        return Result.Ok(result);
    }
}