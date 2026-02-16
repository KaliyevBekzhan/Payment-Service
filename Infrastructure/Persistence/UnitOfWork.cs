using Application.Repositories;
using FluentResults;

namespace Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Result> SaveChangesAsync()
    {
        try
        {
            await _dbContext.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail(e.Message);
        }
    }

    public async Task<Result> BeginTransactionAsync()
    {
        try
        {
            await _dbContext.Database.BeginTransactionAsync();
            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail(e.Message);
        }
    }

    public async Task<Result> CommitTransactionAsync()
    {
        try
        {
            await _dbContext.Database.CommitTransactionAsync();
            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail(e.Message);
        }
    }

    public async Task<Result> RollbackTransactionAsync()
    {
        try
        {
            await _dbContext.Database.RollbackTransactionAsync();
            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail(e.Message);
        }
    }
}