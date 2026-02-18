using Application.Dto;
using Application.Repositories;
using Application.UseCases.Interfaces;
using Domain.Enums;
using FluentResults;

namespace Application.UseCases;

public class DeclinePaymentUseCase : IDeclinePaymentUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGuard _guard;
    
    public DeclinePaymentUseCase(IUnitOfWork unitOfWork, 
        IPaymentRepository paymentRepository, 
        IUserRepository userRepository,
        IGuard guard)
    {
        _unitOfWork = unitOfWork;
        _paymentRepository = paymentRepository;
        _userRepository = userRepository;
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
        
        
        var paymentResult = await _paymentRepository.UpdatePaymentStatus(dto.PaymentId, (int)Statuses.Declined, dto.ChangerId);

        if (paymentResult.IsFailed)
        {
            return Result.Fail(paymentResult.Errors);
        } 
        
        await _unitOfWork.SaveChangesAsync();
        
        return await _unitOfWork.CommitTransactionAsync();
    }
}