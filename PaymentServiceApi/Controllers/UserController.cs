using System.Security.Claims;
using Application.Dto;
using Application.UseCases.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentServiceApi.Attributes;
using PaymentServiceApi.Dto;

namespace PaymentServiceApi.Controllers;

[ApiController]
[Route( "api/v1/[controller]" )]
[Authorize]
[RequireHmac]
public class UserController : Controller
{
    protected int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                                             ?? User.FindFirst("id")?.Value 
                                             ?? "0");
    // GET
    [HttpGet("")]
    public async Task<IActionResult> MyCabinet([FromServices] IMyCabinetUseCase myCabinetUseCase)
    {
        var result = await myCabinetUseCase.ExecuteAsync(CurrentUserId);
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest("Не удалось получить данные для кабинета");
    }

    [HttpGet("history")]
    public async Task<IActionResult> MyActionHistory([FromQuery] PaginationRequest page,
        [FromServices] IMyActionsHistoryUseCase myactionsHistoryUseCase)
    {
        var result = await myactionsHistoryUseCase.ExecuteAsync(new GetMyActionsDto(
            CurrentUserId,
            page.PageNumber,
            page.PageSize
        ));
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest("Не удалось получить данные для истории действий");
    }
}