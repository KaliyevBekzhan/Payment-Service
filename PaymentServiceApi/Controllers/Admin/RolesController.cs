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
public class RolesController : ControllerBase
{
    protected int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                                             ?? User.FindFirst("id")?.Value 
                                             ?? "0");
    
    // GET
    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] AddRoleRequest request,
        [FromServices] IAddRoleUseCase addRoleUseCase)
    {
        var command = new AddRoleDto(request.Name, request.IsAdmin, request.Priority);
        
        var result = await addRoleUseCase.ExecuteAsync(command, CurrentUserId);
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest("Не удалось добавить роль");
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id,
        [FromBody] UpdateRoleRequest request,
        [FromServices] IUpdateRoleUseCase updateRoleUseCase)
    {
        var command = new UpdateRoleDto(request.Name, request.Priority, id);
        
        var result = await updateRoleUseCase.ExecuteAsync(command, CurrentUserId);
        
        return result.IsSuccess ? Ok() : BadRequest("При обновлении произошла ошибка");
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id,
        [FromServices] IDeleteRoleUseCase deleteRoleUseCase)
    {
        var result = await deleteRoleUseCase.ExecuteAsync(id, CurrentUserId);
        
        return result.IsSuccess ? Ok() : BadRequest("При удалении произошла ошибка");
    }
    
    [HttpGet("")]
    public async Task<IActionResult> Index([FromServices] IGetRolesUseCase getRolesUseCase)
    {
        var result = await getRolesUseCase.ExecuteAsync(CurrentUserId);
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest("Не удалось получить список ролей");
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetInfo(int id, 
        [FromServices] IGetRoleInfoForAdminUseCase getRoleInfoForAdminUseCase)
    {
        var result = await getRoleInfoForAdminUseCase.ExecuteAsync(id, CurrentUserId);
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest("Не удалось получить детали роли");
    }
}