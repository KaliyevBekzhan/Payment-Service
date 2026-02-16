using Application.Dto.Returns;

namespace Application.UseCases.Interfaces;

public interface IGetInfoForPaymentUseCase
{
    Task<PaymentInfoDto> GetInfoForPayment(int paymentId);
}