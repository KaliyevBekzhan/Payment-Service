using Application.Dto;
using Application.Repositories;
using Application.UseCases.Interfaces;
using Domain.Entity;
using Domain.Enums;
using Domain.Policies;
using FluentResults;

namespace Application.UseCases;

public class AcceptPaymentUseCase : IAcceptPaymentUseCase
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGuard _guard;
    
    public AcceptPaymentUseCase(IPaymentRepository paymentRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IGuard guard)
    {
        _paymentRepository = paymentRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _guard = guard;
    }
    
    public async Task<Result> ExecuteAsync(UpdatePaymentStatusDto dto)
    {
        var validationResult = await _guard.ValidateAdminRole(dto.ChangerId, _userRepository);
        
        if (validationResult.IsFailed) return validationResult;
        
        await _unitOfWork.BeginTransactionAsync();
        
        var payment = await _paymentRepository.GetPaymentByIdAsync(dto.PaymentId);

        var paymentStatus = _guard.ValidatePaymentStatus(payment);
        
        if (paymentStatus.IsFailed)
        {
            return Result.Fail(payment.Errors);
        }
        
        _guard.ValidatePaymentStatus(payment.Value);
        
        var paymentResult = await _paymentRepository.UpdatePaymentStatus(dto.PaymentId, (int)Statuses.Accepted, dto.ChangerId);
        
        if (paymentResult.IsFailed)
        {
            return Result.Fail(paymentResult.Errors);
        }
        
        var newAccount = PaymentPolicy.ValidateWithdraw(payment.Value);

        if (newAccount.IsFailed)
        {
            return Result.Fail(newAccount.Errors);
        }
        
        var userResult = await _userRepository.UpdateUserAccountAsync(payment.Value.UserId, newAccount.Value);
        
        if (userResult.IsFailed)
        {
            return Result.Fail(userResult.Errors);
        }
        
        return await _unitOfWork.CommitTransactionAsync();
    }
}