using Domain.Entity;
using FluentResults;

namespace Application.Interfaces;

public interface ITopupRepository
{
    Task<Result> AddTopupAsync(TopUp topup);
    Task<Result<IEnumerable<TopUp>>> GetTopupsAsync();
    Task<Result<IEnumerable<TopUp>>> GetTopupsByUserIdAsync (int userId, int pageNumber = 1, int pageSize = 10);
    Task<Result<TopUp>> GetTopupByIdAsync(int id);
}