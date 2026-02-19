using System.Security.Claims;
using Application.Dto;
using Application.UseCases.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentServiceApi.Dto;

namespace PaymentServiceApi.Controllers.Admin;

[Area( "Admin")]
[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/v1/[area]/[controller]")]
public class CurrenciesController : ControllerBase
{
    protected int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                                             ?? User.FindFirst("id")?.Value 
                                             ?? "0");
    
    // GET
    [HttpGet("")]
    public async Task<IActionResult> Index([FromServices] IGetCurrenciesForAdminUseCase getAllCurrenciesUseCase)
    {
        var result = await getAllCurrenciesUseCase.ExecuteAsync(CurrentUserId);
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest("Не удалось получить список валют");
    }
    
    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] AddCurrencyRequest request,
        [FromServices] IAddCurrencyUseCase addCurrencyUseCase)
    {
        var command = new AddCurrencyDto(request.Name, request.Rate);
        
        var result = await addCurrencyUseCase.ExecuteAsync(command, CurrentUserId);
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest("Не удалось добавить валюту");
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id,
        [FromBody] UpdateCurrencyRequest request,
        [FromServices] IUpdateCurrencyUseCase updateCurrencyUseCase)
    {
        var command = new UpdateCurrencyDto(id, request.Name, request.Rate);
        
        var result = await updateCurrencyUseCase.ExecuteAsync(command, CurrentUserId);
        
        return result.IsSuccess ? Ok() : BadRequest("При обновлении валюты произошла ошибка");
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id,
        [FromServices] IDeleteCurrencyUseCase deleteCurrencyUseCase)
    {
        var result = await deleteCurrencyUseCase.ExecuteAsync(id, CurrentUserId);
        
        return result.IsSuccess ? Ok() : BadRequest("При удалении валюты произошла ошибка");
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetInfo(int id,
        [FromServices] IGetCurrencyInfoUseCase getCurrencyInfoUseCase)
    {
        var result = await getCurrencyInfoUseCase.ExecuteAsync(id, CurrentUserId);
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest("Не удалось получить детали валюты");
    }
}