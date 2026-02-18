using FluentResults;

namespace Application.Repositories;

public interface IBaseRepository<T> where T : class
{
    Task<Result<T>> GetByIdAsync(int id);
    Task<Result<IEnumerable<T>>> GetAllAsync();
    Task<Result<T>> AddAsync(T entity);
    Task<Result> UpdateAsync(T entity);
    Task<Result> DeleteAsync(int id);
}