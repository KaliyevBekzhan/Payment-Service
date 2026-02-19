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
    
    public GetMyPaymentsUseCase(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }
    
    public async Task<Result<IEnumerable<PaymentDto>>> ExecuteAsync(GetMyPaymentsDto dto)
    {
        var payments = await _paymentRepository.GetPaymentsByUserIdAsync(dto.UserId);

        var result = payments.Value
            .Select(payment => new PaymentDto(
                payment.Id,
                (payment.OriginalAmount) * -1, 
                payment.Currency.Name, 
                (payment.AmountInTenge) * -1,
                payment.Status.Name,
                payment.Comment,
                payment.CreatedAt
            )).ToList();

        return Result.Ok(result.AsEnumerable());
    }
}