using System.Security.Claims;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace PaymentServiceApi.Controllers;

[ApiController]
[Route("admin/payments/v1")]
public class AdminPaymentsController : ControllerBase
{
    protected int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                                             ?? User.FindFirst("id")?.Value 
                                             ?? "0");
    // GET
    public async Task<IActionResult> Index()
    {
        return Ok();
    }
}