using FluentResults;

namespace Application.Repositories;

public interface IUnitOfWork
{
    Task<Result> SaveChangesAsync();
    
    // Транзакционные методы, если понадобятся
    Task<Result> BeginTransactionAsync();
    Task<Result> CommitTransactionAsync();
    Task<Result> RollbackTransactionAsync();
}