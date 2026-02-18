using Application.Dto.Returns;
using Application.Repositories;
using Application.UseCases.Interfaces;
using FluentResults;

namespace Application.UseCases.Admin;

public class GetPaymentInfoForAdminUseCase : IGetPaymentInfoForAdminUseCase
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IGuard _guard;
    private readonly IUserRepository _userRepository;
    
    public GetPaymentInfoForAdminUseCase(IPaymentRepository paymentRepository, IGuard guard, IUserRepository userRepository)
    {
        _paymentRepository = paymentRepository;
        _guard = guard;
        _userRepository = userRepository;
    }


    public async Task<Result<PaymentInfoForAdminDto>> ExecuteAsync(int paymentId, int userId)
    {
        var validationResult = await _guard.ValidateAdminRole(userId, _userRepository);

        if (validationResult.IsFailed)
        {
            return Result.Fail(validationResult.Errors);
        }

        var paymentResult = await _paymentRepository.GetPaymentByIdForAdminAsync(paymentId);

        if (paymentResult.IsFailed)
        {
            return Result.Fail(paymentResult.Errors);
        }

        var payment = paymentResult.Value;
        
        return Result.Ok(new PaymentInfoForAdminDto(
            payment.Id,
            payment.UserId,
            payment.User.Name,
            payment.OriginalAmount,
            payment.Currency.Name,
            payment.AmountInTenge,
            payment.Account,
            payment.CreatedAt,
            payment.WalletNumber,
            payment.Comment,
            payment.Status.Name,
            payment.ChangerId,
            payment.Changer?.Name
        ));
    }
}