using Application.Repositories;
using Application.UseCases.Interfaces;
using Domain.Entity;
using FluentResults;

namespace Application.UseCases;

public class DeleteCurrencyUseCase : IDeleteCurrencyUseCase
{
    private readonly IBaseRepository<Currency> _currencyRepository;
    private readonly IGuard _guard;
    private readonly IUserRepository _userRepository;
    
    public DeleteCurrencyUseCase(IBaseRepository<Currency> currencyRepository, IUserRepository userRepository, IGuard guard)
    {
        _currencyRepository = currencyRepository;
        _userRepository = userRepository;
        _guard = guard;
    }
    
    public async Task<Result> ExecuteAsync(int id, int userId)
    {
        var validationResult = await _guard.ValidateAdminRole(userId, _userRepository);
        
        if (validationResult.IsFailed) return validationResult;
        
        var result = await _currencyRepository.DeleteAsync(id);
        
        return result;
    }
}