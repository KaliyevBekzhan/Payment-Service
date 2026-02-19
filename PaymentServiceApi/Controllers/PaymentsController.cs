using System.Security.Claims;
using Application.Dto;
using Application.UseCases;
using Application.UseCases.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentServiceApi.Dto;

namespace PaymentServiceApi.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class PaymentsController : ControllerBase
{
    protected int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                                             ?? User.FindFirst("id")?.Value 
                                             ?? "0");
    
    // GET
    [HttpPost("")]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request,
        [FromServices] IAddPaymentUseCase addPaymentUseCase)
    {
        var command = new AddPaymentDto(CurrentUserId, request.OriginalAmount, request.CurrencyId, request.Comment);
        
        var result = await addPaymentUseCase.ExecuteAsync(command);
        
        return result.IsSuccess ? Ok() : BadRequest("Не удалось создать платеж");
    }

    [HttpGet("")]
    public async Task<IActionResult> GetMyPayments([FromServices] IGetMyPaymentsUseCase getMyPaymentsUseCase)
    {
        var result = await getMyPaymentsUseCase.ExecuteAsync(new GetMyPaymentsDto(CurrentUserId));
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest("Не удалось получить платежи");
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPaymentInfo(int id,
        [FromServices] IGetInfoForPaymentUseCase getInfoForPaymentUseCase)
    {
        var result = await getInfoForPaymentUseCase.ExecuteAsync(id);
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest("Не удалось получить данные платежа");
    }
}