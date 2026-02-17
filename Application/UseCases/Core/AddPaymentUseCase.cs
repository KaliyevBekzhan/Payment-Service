using Application.Dto;
using Application.Repositories;
using Application.UseCases.Interfaces;
using Domain.Entity;
using Domain.Enums;
using Domain.Policies;
using FluentResults;

namespace Application.UseCases;

public class AddPaymentUseCase : IAddPaymentUseCase
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseRepository<Currency> _currencyRepository;
    
    public AddPaymentUseCase(IPaymentRepository paymentRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IBaseRepository<Currency> currencyRepository)
    {
        _paymentRepository = paymentRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _currencyRepository = currencyRepository;
    }
    public async Task<Result> ExecuteAsync(AddPaymentDto dto)
    {
        var result = await _userRepository.GetUserByIdAsync(dto.UserId);

        if (result.IsFailed)
        {
            return Result.Fail(result.Errors);
        }

        var currencyResult = await _currencyRepository.GetByIdAsync(dto.CurrencyId);

        if (currencyResult.IsFailed)
        {
            return currencyResult.ToResult();
        }
        
        var user = result.Value;

        var payment = new Payment
        {
            UserId = dto.UserId,
            Account = user.Account,
            WalletNumber = user.WalletNumber,
            OriginalAmount = dto.Amount,
            AmountInTenge = ConverterPolicy.ConvertToTenge(currencyResult.Value, dto.Amount),
            CurrencyName = currencyResult.Value.Name,
            CurrencyId = dto.CurrencyId,
            Comment = dto.Comment,
            StatusId = (int)Statuses.Created,
            ChangerId = null
        };
        
        await _paymentRepository.AddPaymentAsync(payment);
        
        return await _unitOfWork.SaveChangesAsync();
    }
}