using Application.Dto.Returns;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IGetInfoForPaymentUseCase
{
    Task<Result<ActionsDto>> ExecuteAsync(int paymentId);
}