using System.Security.Claims;
using Application.Dto;
using Application.UseCases.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentServiceApi.Attributes;
using PaymentServiceApi.Dto;

namespace PaymentServiceApi.Controllers;

[ApiController]
[Authorize]
[Route( "api/v1/[controller]" )]
[RequireHmac]
public class TopupController : Controller
{
    protected int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                                             ?? User.FindFirst("id")?.Value 
                                             ?? "0");
    
    [HttpPost]
    public async Task<IActionResult> CreateTopup([FromBody] AddTopUpRequest request,
        [FromServices] IAddTopupUseCase addTopupUseCase)
    {
        var result = await addTopupUseCase.ExecuteAsync(new AddTopupDto(
            CurrentUserId, 
            request.OriginalAmount, 
            request.CurrencyId, 
            request.Comment
        ));
        
        return result.IsSuccess ? Ok() : BadRequest("Не удалось пополнить счет");
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTopupInfo(int id,
        [FromServices] IGetTopupInfoUseCase getTopupInfoUseCase)
    {
        var result = await getTopupInfoUseCase.ExecuteAsync(id);
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }

    [HttpGet()]
    public async Task<IActionResult> GetMyPopups([FromQuery] PaginationRequest page,
        [FromServices] IGetMyTopupsUseCase getMyTopupsUseCase)
    {
        var result = await getMyTopupsUseCase.ExecuteAsync(
            new GetMyActionsDto(
                CurrentUserId, 
                page.PageNumber, 
                page.PageSize
            ));
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }
}