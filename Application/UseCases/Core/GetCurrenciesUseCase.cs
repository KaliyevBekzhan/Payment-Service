using Application.Dto;
using Application.Repositories;
using Application.UseCases.Interfaces;
using Domain.Entity;
using FluentResults;

namespace Application.UseCases;

public class GetCurrenciesUseCase : IGetCurrenciesUseCase
{
    private readonly IBaseRepository<Currency> _currencyRepository;
    
    public GetCurrenciesUseCase(IBaseRepository<Currency> currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public async Task<Result<IEnumerable<CurrenciesForUserDto>>> ExecuteAsync()
    {
        var currenciesResult = await _currencyRepository.GetAllAsync();
        
        if (currenciesResult.IsFailed)
        {
            return Result.Fail(currenciesResult.Errors);
        }
        
        var currencyNames = currenciesResult.Value.Select(c => new CurrenciesForUserDto(
            id: c.Id,
            Name: c.Name
        ));
        
        return Result.Ok(currencyNames);
    }
}