using Domain.Entity;
using FluentResults;

namespace Domain.Policies;

public static class PaymentPolicy
{
    public static Result<decimal> ValidateWithdraw(Payment payment)
    {
        if (payment.OriginalAmount < 0)
        {
            return Result.Fail("Payment amount cannot be negative");
        }
        
        if (payment.Account - payment.OriginalAmount < 0)
        {
            return Result.Fail("Insufficient funds");
        }

        return Result.Ok(payment.Account - payment.OriginalAmount);
    }
}