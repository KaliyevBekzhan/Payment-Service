using Application.Dto;
using Application.Dto.Returns;
using Domain.Entity;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IGetMyPaymentsUseCase
{
    Task<Result<IEnumerable<PaymentDto>>> ExecuteAsync(GetMyPaymentsDto dto);
}