using Application.Dto;
using Application.Dto.Returns;
using Application.Repositories;
using Application.UseCases.Interfaces;
using Domain.Entity;
using FluentResults;

namespace Application.UseCases;

public class GetMyPaymentsUseCase : IGetMyPaymentsUseCase
{
    private readonly IPaymentRepository _paymentRepository;
    public async Task<Result<IEnumerable<PaymentDto>>> ExecuteAsync(GetMyPaymentsDto dto)
    {
        var payments = await _paymentRepository.GetPaymentsByUserIdAsync(dto.UserId);

        IEnumerable<PaymentDto> result = payments.Value
            .Select(payment => new PaymentDto(payment.OriginalAmount, payment.Currency.Name, payment.Status.Name));

        return Result.Ok(result);
    }
}