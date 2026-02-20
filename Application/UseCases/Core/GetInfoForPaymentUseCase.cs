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
    public async Task<Result<ActionsDto>> ExecuteAsync(int paymentId)
    {
        var payment = await _paymentRepository.GetPaymentByIdAsync(paymentId);
        
        if (payment.IsFailed) return Result.Fail<ActionsDto>(payment.Errors);
        
        return Result.Ok(new ActionsDto(
            payment.Value.Id, 
            payment.Value.OriginalAmount, 
            payment.Value.Currency.Name,
            payment.Value.AmountInTenge,
            payment.Value.Status.Name,
            payment.Value.Account,
            payment.Value.Comment,
            "payment",
            payment.Value.CreatedAt
        ));
    }
}