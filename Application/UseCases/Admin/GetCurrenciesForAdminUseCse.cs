using Application.Dto;
using Application.Dto.Returns;
using Application.Repositories;
using Application.UseCases.Interfaces;
using Domain.Entity;
using FluentResults;

namespace Application.UseCases.Admin;

public class GetCurrenciesForAdminUseCse : IGetCurrenciesForAdminUseCase
{
    private readonly IBaseRepository<Currency> _currencyRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGuard _guard;

    public GetCurrenciesForAdminUseCse(
        IBaseRepository<Currency> currencyRepository,
        IUserRepository userRepository,
        IGuard guard)
    {
        _currencyRepository = currencyRepository;
        _userRepository = userRepository;
        _guard = guard;
    }
    
    public async Task<Result<IEnumerable<CurrenciesDto>>> ExecuteAsync(int userId)
    {
        var validationResult = await _guard.ValidateAdminRole(userId, _userRepository);
        
        if (validationResult.IsFailed) return validationResult;
        
        var currencies = await _currencyRepository.GetAllAsync();
        
        if (currencies.IsFailed) return Result.Fail(currencies.Errors);
        
        var result = currencies.Value.Select(c => new CurrenciesDto(c.Id, c.Name, c.ConversionRate));
        
        return Result.Ok(result);
    }
}
