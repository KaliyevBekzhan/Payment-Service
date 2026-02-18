using Application.Dto.Returns;
using Application.Repositories;
using Application.UseCases.Interfaces;
using Domain.Entity;
using FluentResults;

namespace Application.UseCases.Admin;

public class GetCurrencyInfoUseCase : IGetCurrencyInfoUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IGuard _guard;
    private readonly IBaseRepository<Currency> _currencyRepository;
    
    public GetCurrencyInfoUseCase(IUserRepository userRepository, IGuard guard, IBaseRepository<Currency> currencyRepository)
    {
        _userRepository = userRepository;
        _guard = guard;
        _currencyRepository = currencyRepository;
    }
    
    public async Task<Result<CurrencyInfoDto>> ExecuteAsync(int currencyId, int userId)
    {
        var validation = await _guard.ValidateAdminRole(userId, _userRepository);
        if (validation.IsFailed)
        {
            return Result.Fail(validation.Errors);
        }
        
        var result = await _currencyRepository.GetByIdAsync(currencyId);
        if (result.IsFailed)
        {
            return Result.Fail(result.Errors);
        }
        
        return Result.Ok(new CurrencyInfoDto(
            result.Value.Id,
            result.Value.Name,
            result.Value.ConversionRate
        ));
    }
}