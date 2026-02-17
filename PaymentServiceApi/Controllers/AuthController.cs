using Application.Dto;
using Application.Dto.Returns;
using Application.UseCases.Interfaces;
using Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PaymentServiceApi.Controllers;

[ApiController]
[Route("[controller]/v1")]
public class AuthController : ControllerBase
{
    // GET
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody]LoginDto dto,
        [FromServices] ILoginUserUseCase loginUserUseCase)
    { 
        var tokenResult = await loginUserUseCase.Execute(dto);

        if (tokenResult.IsFailed) return Unauthorized("Во время входа произошла ошибка");
        
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = tokenResult.Value.Expires,
            SameSite = SameSiteMode.Strict
        };
        
        Response.Cookies.Append("Authorization", tokenResult.Value.Token, cookieOptions);
        
        return Ok(tokenResult.Value.Role);
    }
    
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto,
        [FromServices] IRegisterUserUseCase registerUserUseCase)
    {
        var result = await registerUserUseCase.ExecuteAsync(dto);
        
        if (result.IsFailed) return BadRequest("Во время регистрации произошла ошибка");
        
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = result.Value.Expires,
            SameSite = SameSiteMode.Strict
        };
        
        Response.Cookies.Append("Authorization", result.Value.Token, cookieOptions);

        var response = new
        {
            result.Value.Name,
            result.Value.Role,
            result.Value.WalletNumber,
            result.Value.Account
        };
        
        return Ok(response);
    }
    
    [HttpPost("/logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        Response.Cookies.Delete("Authorization");
        
        return Ok();
    }
}