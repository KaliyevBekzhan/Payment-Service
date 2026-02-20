using System.Security.Claims;
using Application.UseCases.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentServiceApi.Attributes;

namespace PaymentServiceApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
[RequireHmac]
public class CurrenciesController : ControllerBase
{
    
    [HttpGet]
    public async Task<IActionResult> GetCurrencies([FromServices] IGetCurrenciesUseCase getCurrenciesUseCase)
    {
        var result = await getCurrenciesUseCase.ExecuteAsync();
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest("Не удалось получить список валют");
    }
}