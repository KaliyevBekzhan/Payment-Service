using System.Security.Claims;
using Application.UseCases.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PaymentServiceApi.Controllers;

[ApiController]
[Route( "api/v1/[controller]" )]
[Authorize]
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
    public async Task<IActionResult> MyActionHistory([FromServices] IMyActionsHistoryUseCase myactionsHistoryUseCase)
    {
        var result = await myactionsHistoryUseCase.ExecuteAsync(CurrentUserId);
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest("Не удалось получить данные для истории действий");
    }
}