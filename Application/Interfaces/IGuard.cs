using Application.Dto;
using Domain.Entity;
using FluentResults;

namespace Application.Repositories;

public interface IGuard
{
    Result<User> AuthUserValidate(Result<User> result);
    Result RegisterUserValidate(RegisterUserDto user);
    Result ValidatePaymentStatus(Result<Payment> payment);
}