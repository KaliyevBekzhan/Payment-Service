using System.Security.Claims;
using Application.Dto;
using Application.UseCases.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentServiceApi.Dto;

namespace PaymentServiceApi.Controllers;

[ApiController]
[Authorize]
[Route("[controller]/v1")]
public class PaymentsController : ControllerBase
{
    protected int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                                             ?? User.FindFirst("id")?.Value 
                                             ?? "0");
    
    // GET
    [HttpPost("create")]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDto dto,
        [FromServices] IAddPaymentUseCase addPaymentUseCase)
    {
        var command = new AddPaymentDto(CurrentUserId, dto.Amount, dto.CurrencyId, dto.Comment);
        
        var result = await addPaymentUseCase.ExecuteAsync(command);
        
        return result.IsSuccess ? Ok() : BadRequest("Не удалось создать платеж");
    }
}