using Application.Dto;
using Application.Repositories;
using Application.UseCases.Interfaces;
using Domain.Entity;
using FluentResults;

namespace Application.UseCases;

public class AddCurrencyUseCase : IAddCurrencyUseCase
{
    private readonly IBaseRepository<Currency> _currencyRepository;
    private readonly IGuard _guard;
    private readonly IUserRepository _userRepository;
    public AddCurrencyUseCase(IBaseRepository<Currency> currencyRepository, IUserRepository userRepository, IGuard guard)
    {
        _currencyRepository = currencyRepository;
        _userRepository = userRepository;
        _guard = guard;
    }
    public async Task<Result> ExecuteAsync(AddCurrencyDto dto, int userId)
    {
        var validationResult = await _guard.ValidateAdminRole(userId, _userRepository);
        
        if (validationResult.IsFailed) return validationResult;
        
        var currency = new Currency{
            Name = dto.name,
            ConversionRate = dto.rate
        };
        
        var result = await _currencyRepository.AddAsync(currency);
        
        return result;
    }
}