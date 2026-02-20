using Application.Dto;
using Application.Dto.Returns;
using Application.Interfaces;
using Application.UseCases.Interfaces;
using FluentResults;

namespace Application.UseCases;

public class GetMyTopupsUseCase : IGetMyTopupsUseCase
{
    private readonly ITopupRepository _topupRepository;
    
    public GetMyTopupsUseCase(ITopupRepository topupRepository)
    {
        _topupRepository = topupRepository;
    }
    
    public async Task<Result<IEnumerable<ActionsDto>>> ExecuteAsync(GetMyActionsDto dto)
    {
        var result = await _topupRepository.GetTopupsByUserIdAsync(dto.UserId, dto.PageNumber, dto.PageSize);

        if (result.IsFailed)
        {
            return Result.Fail(result.Errors);
        }
        
        return Result.Ok(result.Value.Select(t => new ActionsDto(
            t.Id,
            t.OriginalAmount, 
            t.Currency.Name,
            t.AmountInTenge,
            t.Status.Name,
            t.Account,
            t.Comment,
            "TopUp",
            t.CreatedAt)
        ));
    }
}