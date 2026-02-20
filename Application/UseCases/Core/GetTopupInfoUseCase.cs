using Application.Dto;
using Application.Dto.Returns;
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
    
    public async Task<Result<ActionsDto>> ExecuteAsync(int topupId)
    {
        var result = await _topupRepository.GetTopupByIdAsync(topupId);
        
        if (result.IsFailed)
        {
            return Result.Fail(result.Errors);
        }

        return Result.Ok(new ActionsDto(
            result.Value.Id,
            result.Value.OriginalAmount,
            result.Value.Currency.Name,
            result.Value.AmountInTenge,
            result.Value.Status.Name,
            result.Value.Account,
            result.Value.Comment,
            "TopUp",
            result.Value.CreatedAt
        ));
    }
}