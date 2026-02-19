using Application.Dto;
using Application.Interfaces;
using Application.UseCases.Interfaces;
using FluentResults;

namespace Application.UseCases;

public class GetTopupInfoUseCase : IGetTopupInfoUseCase
{
    private readonly ITopupRepository _topupRepository;
    
    public GetTopupInfoUseCase(ITopupRepository topupRepository)
    {
        _topupRepository = topupRepository;
    }
    
    public async Task<Result<TopupInfoDto>> ExecuteAsync(int topupId)
    {
        var result = await _topupRepository.GetTopupByIdAsync(topupId);
        
        if (result.IsFailed)
        {
            return Result.Fail(result.Errors);
        }

        return Result.Ok(new TopupInfoDto(
            result.Value.Id,
            result.Value.OriginalAmount,
            result.Value.Currency.Name,
            result.Value.Status.Name,
            result.Value.AmountInTenge,
            result.Value.Account,
            result.Value.CreatedAt
        ));
    }
}