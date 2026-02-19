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
    
    public async Task<Result<IEnumerable<MyTopupsDto>>> ExecuteAsync(int userId)
    {
        var result = await _topupRepository.GetTopupByUserIdAsync(userId);

        if (result.IsFailed)
        {
            return Result.Fail(result.Errors);
        }
        
        return Result.Ok(result.Value.Select(t => new MyTopupsDto(
            t.Id,
            t.OriginalAmount, 
            t.AmountInTenge, 
            t.Currency.Name, 
            t.Comment, 
            t.Status.Name,
            t.CreatedAt)
        ));
    }
}