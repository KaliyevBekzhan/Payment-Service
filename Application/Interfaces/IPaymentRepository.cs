using Domain.Entity;
using FluentResults;

namespace Application.Repositories;

public interface IPaymentRepository
{
    Task AddPaymentAsync(Payment payment);
    Task<Result<IEnumerable<Payment>>> GetPaymentsByUserIdAsync(int userId, int pageNumber = 1, int pageSize = 10);
    Task<Result> UpdatePaymentStatus(int id, int statusId, int userId);
    Task<Result<Payment>> GetPaymentByIdAsync(int id);
    Task<Result<Payment>> GetPaymentByIdForAdminAsync(int id);
    Task<Result<IEnumerable<Payment>>> GetPaymentsByStatusIdAsync(int statusId);
}