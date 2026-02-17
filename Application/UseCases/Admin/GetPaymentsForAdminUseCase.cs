using Application.Dto.Returns;
using Application.Repositories;
using Application.UseCases.Interfaces;
using Domain.Enums;
using FluentResults;

namespace Application.UseCases;

public class GetPaymentsForAdminUseCase : IGetPaymentsForAdminUseCase
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGuard _guard;
    
    public GetPaymentsForAdminUseCase(IPaymentRepository paymentRepository, IUserRepository userRepository, IGuard guard)
    {
        _paymentRepository = paymentRepository;
        _userRepository = userRepository;
    }
    
    public async Task<Result<IEnumerable<AdminPaymentsDto>>> ExecuteAsync(int userId)
    {
        var validationResult = await _guard.ValidateAdminRole(userId, _userRepository);
        
        if (validationResult.IsFailed) return validationResult;
        
        var paymentsResult = await _paymentRepository.GetPaymentsByStatusIdAsync((int)Statuses.Created);

        if (paymentsResult.IsFailed)
        {
            return Result.Fail(paymentsResult.Errors);
        }
        
        var result = paymentsResult.Value
            .Select(x => new AdminPaymentsDto(
                x.User.Name,
                x.OriginalAmount,
                x.AmountInTenge,
                x.Account,
                x.WalletNumber,
                x.Currency.Name,
                x.Id,
                x.User.Role.Name,
                x.CreatedAt));
        
        return Result.Ok(result);
    }
}