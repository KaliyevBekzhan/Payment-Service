using System.Text.RegularExpressions;
using Application.Dto;
using Application.Repositories;
using Domain.Entity;
using Domain.Enums;
using FluentResults;

namespace Infrastructure.Services;

public class Guard : IGuard
{
    public Result<User> AuthUserValidate(Result<User> result)
    {
        if (result.IsFailed)
        {
            return result;
        }
        
        if (result.Value == null)
        {
            return Result.Fail(result.Errors);
        }
        
        return result;
    }

    public Result RegisterUserValidate(RegisterUserDto user)
    {
        var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$");
        
        if (user.Iin.Length != 12)
        {
            return Result.Fail("IIN must be 12 characters long");
        }
        
        if (user.Password.Length < 8)
        {
            return Result.Fail("Password must be at least 8 characters long");
        }

        if (!passwordRegex.IsMatch(user.Password))
        {
            return Result.Fail("Password must contain at least one uppercase letter, one lowercase letter and one digit");
        }
        
        return Result.Ok();
    }

    public Result ValidatePaymentStatus(Result<Payment> payment)
    {
        if (payment.IsFailed)
        {
            return Result.Fail(payment.Errors);
        }
        
        if (payment.Value.StatusId == (int)Statuses.Declined)
        {
            return Result.Fail("Payment already declined");
        } 
        else if (payment.Value.StatusId == (int)Statuses.Accepted)
        {
            return Result.Fail("Payment already accepted");
        }
        
        return Result.Ok();
    }
}