using Domain.Entity;
using FluentResults;

namespace Application.Interfaces;

public interface ITopupRepository
{
    Task<Result> AddTopupAsync(TopUp topup);
    Task<Result<IEnumerable<TopUp>>> GetTopupsAsync();
    Task<Result<IEnumerable<TopUp>>> GetTopupByUserIdAsync (int userId);
    Task<Result<TopUp>> GetTopupByIdAsync(int id);
}