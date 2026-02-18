using Application.Dto.Returns;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IGetPaymentInfoForAdminUseCase
{
    Task<Result<PaymentInfoForAdminDto>> ExecuteAsync(int paymentId, int userId);
}