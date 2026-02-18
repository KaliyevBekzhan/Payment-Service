using Application.Dto.Returns;
using Application.Repositories;
using Application.UseCases.Interfaces;
using FluentResults;

namespace Application.UseCases;

public class GetInfoForPaymentUseCase : IGetInfoForPaymentUseCase
{
    private readonly IPaymentRepository _paymentRepository;
    public GetInfoForPaymentUseCase(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }
    public async Task<Result<PaymentInfoDto>> ExecuteAsync(int paymentId)
    {
        var payment = await _paymentRepository.GetPaymentByIdAsync(paymentId);
        
        if (payment.IsFailed) return Result.Fail<PaymentInfoDto>(payment.Errors);
        
        return Result.Ok(new PaymentInfoDto(
            payment.Value.Id, 
            payment.Value.OriginalAmount, 
            payment.Value.Currency.Name,
            payment.Value.Status.Name,
            payment.Value.AmountInTenge,
            payment.Value.Account,
            payment.Value.CreatedAt
        ));
    }
}