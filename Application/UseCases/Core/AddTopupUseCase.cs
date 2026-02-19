using Application.Dto;
using Application.Interfaces;
using Application.Repositories;
using Application.UseCases.Interfaces;
using Domain.Entity;
using Domain.Enums;
using Domain.Policies;
using FluentResults;

namespace Application.UseCases;

public class AddTopupUseCase : IAddTopupUseCase
{
    private readonly ITopupRepository _topupRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBaseRepository<Currency> _currencyRepository;
    private readonly IUnitOfWork _unitOfWork;
    public AddTopupUseCase(ITopupRepository topupRepository, 
        IUserRepository userRepository, 
        IBaseRepository<Currency> currencyRepository, 
        IUnitOfWork unitOfWork)
    {
        _topupRepository = topupRepository;
        _userRepository = userRepository;
        _currencyRepository = currencyRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> ExecuteAsync(AddTopupDto dto)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        var userResult = await _userRepository.GetUserByIdAsync(dto.UserId);
        if (userResult.IsFailed)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return Result.Fail(userResult.Errors);
        }
        
        var currencyResult = await _currencyRepository.GetByIdAsync(dto.CurrencyId);

        if (currencyResult.IsFailed)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return Result.Fail(currencyResult.Errors);
        }
        
        var amountInTenge = ConverterPolicy.ConvertToTenge(currencyResult.Value, dto.OriginalAmount);
        
        var newAccount = TopUpPolicy.ValidateTopUp(userResult.Value, amountInTenge);

        if (newAccount.IsFailed)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return Result.Fail(newAccount.Errors);
        }
        
        var topUp = new TopUp
        {
            UserId = dto.UserId,
            OriginalAmount = dto.OriginalAmount,
            CurrencyId = dto.CurrencyId,
            AmountInTenge = amountInTenge,
            Comment = dto.Comment,
            WalletNumber = userResult.Value.WalletNumber,
            CreatedAt = DateTime.UtcNow,
            StatusId = (int)Statuses.Created,
            CurrencyName = currencyResult.Value.Name,
            Account = userResult.Value.Account,
        };
        
        var topupResult = await _topupRepository.AddTopupAsync(topUp);

        if (topupResult.IsFailed)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return Result.Fail(topupResult.Errors);
        }
        
        var updateUser = await _userRepository.UpdateUserAccountAsync(dto.UserId, newAccount.Value);

        if (updateUser.IsFailed)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return Result.Fail(updateUser.Errors);
        }
        
        await _unitOfWork.SaveChangesAsync();
        
        return await _unitOfWork.CommitTransactionAsync();
    }
}