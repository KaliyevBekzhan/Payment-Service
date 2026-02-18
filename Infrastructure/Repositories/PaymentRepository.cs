using Application.Repositories;
using Domain.Entity;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _dbContext;
    public PaymentRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task AddPaymentAsync(Payment payment)
    {
        await _dbContext.Payments.AddAsync(payment);
    }

    public async Task<Result<IEnumerable<Payment>>> GetPaymentsByUserIdAsync(int userId)
    {
        var result = await _dbContext.Payments.Where(p => p.UserId == userId)
            .Include(p => p.Status)
            .Include(p => p.Currency)
            .ToListAsync();

        if (result.Count == 0)
        {
            return Result.Fail($"No payments found for User {userId}");
        }
        
        return Result.Ok(result.AsEnumerable());
    }

    public async Task<Result> UpdatePaymentStatus(int id, int statusId, int userId)
    {
        var paymentResult = await GetPaymentByIdAsync(id);

        if (paymentResult.IsFailed)
        {
            return Result.Fail(paymentResult.Errors);
        }
        
        var payment = paymentResult.Value;
        
        payment.StatusId = statusId;
        payment.ChangerId = userId;
        
        return Result.Ok();
    }

    public async Task<Result<Payment>> GetPaymentByIdAsync(int id)
    {
        var result = await _dbContext.Payments
            .Include(p => p.Status)
            .Include(p => p.Currency)
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (result == null)
        {
            return Result.Fail("Payment not found");
        }
        
        return Result.Ok(result);
    }

    public async Task<Result<Payment>> GetPaymentByIdForAdminAsync(int id)
    {
        var result = await _dbContext.Payments
            .Include(p => p.Status)
            .Include(p => p.Currency)
            .Include(p => p.User)
            .Include(p => p.Changer)
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (result == null)
        {
            return Result.Fail("Payment not found");
        }
        
        return Result.Ok(result);
    }
    
    public async Task<Result<IEnumerable<Payment>>> GetPaymentsByStatusIdAsync(int statusId)
    {
        var result = await _dbContext.Payments.Where(p => p.StatusId == statusId)
            .Include(p => p.Currency)
            .Include(p => p.User)
            .ThenInclude(u => u.Role)
            .Include(p => p.Status)
            .Where(p => p.StatusId == statusId)
            .OrderByDescending(p => p.User.Role.Priority)
            .ThenByDescending(p => p.CreatedAt)
            .ToListAsync();
        
        return Result.Ok(result.AsEnumerable());
    }
}