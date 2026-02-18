using Application.Dto;
using Application.Repositories;
using Application.UseCases.Interfaces;
using Domain.Entity;
using FluentResults;

namespace Application.UseCases;

public class UpdateCurrencyUseCase : IUpdateCurrencyUseCase
{
    private readonly IBaseRepository<Currency> _currencyRepository;
    private readonly IGuard _guard;
    private readonly IUserRepository _userRepository;
    
    public UpdateCurrencyUseCase(IBaseRepository<Currency> currencyRepository, IUserRepository userRepository, IGuard guard)
    {
        _currencyRepository = currencyRepository;
        _userRepository = userRepository;
        _guard = guard;
    }
    public async Task<Result> ExecuteAsync(UpdateCurrencyDto dto, int userId)
    {
        var validationResult = await _guard.ValidateAdminRole(userId, _userRepository);
        
        if (validationResult.IsFailed) return validationResult;
        
        var currency = new Currency{
            Id = dto.Id,
            Name = dto.Name,
            ConversionRate = dto.Rate
        };
        
        var result = await _currencyRepository.UpdateAsync(currency);
        
        return result;
    }
}