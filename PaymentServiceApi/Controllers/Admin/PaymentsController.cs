using System.Security.Claims;
using Application.Dto;
using Application.UseCases.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentServiceApi.Attributes;

namespace PaymentServiceApi.Controllers.Admin;

[Area( "Admin")]
[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/v1/[area]/[controller]")]
[RequireHmac]
public class PaymentsController : ControllerBase
{
    protected int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                                             ?? User.FindFirst("id")?.Value 
                                             ?? "0");
    // GET
    [HttpPost("{id:int}/accept")]
    public async Task<IActionResult> AcceptPayment(int id,
        [FromServices] IAcceptPaymentUseCase acceptPaymentUseCase)
    {
        var command = new UpdatePaymentStatusDto(id, CurrentUserId);

        var result = await acceptPaymentUseCase.ExecuteAsync(command);
        
        return result.IsSuccess ? Ok() : BadRequest("Не удалось принять платеж");
    }
    
    [HttpPost("{id:int}/decline")]
    public async Task<IActionResult> DeclinePayment(int id,
        [FromServices] IDeclinePaymentUseCase declinePaymentUseCase)
    {
        var command = new UpdatePaymentStatusDto(id, CurrentUserId);

        var result = await declinePaymentUseCase.ExecuteAsync(command);
        
        return result.IsSuccess ? Ok() : BadRequest("Не удалось принять платеж");
    }

    [HttpGet("")]
    public async Task<IActionResult> GetPayments ([FromServices] IGetPaymentsForAdminUseCase getPaymentsForAdminUseCase)
    {
        var result = await getPaymentsForAdminUseCase.ExecuteAsync(CurrentUserId);
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest("Не удалось получить платежи");
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPaymentInfo(int id,
        [FromServices] IGetPaymentInfoForAdminUseCase getPaymentInfoForAdminUseCase)
    {
        var result = await getPaymentInfoForAdminUseCase.ExecuteAsync(id, CurrentUserId);
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest("Не удалось получить информацию о платеже");
    }
}