using Domain.Entity;
using FluentResults;

namespace Domain.Policies;

public static class TopUpPolicy
{
    public static Result<decimal> ValidateTopUp(User user, decimal amount)
    {
        if (amount <= 0)
        {
            return Result.Fail("Сумма пополнения не может быть негативной или нулем");
        }

        if (user.Account + amount > 5000000)
        {
            return Result.Fail("Превышен лимит баланса");
        }

        return Result.Ok(user.Account + amount);
    }
}