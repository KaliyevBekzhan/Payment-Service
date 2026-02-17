using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace PaymentServiceApi.Middlewares;

public class ResultFilter : IActionFilter
{
    private readonly ILogger<ResultFilter> _logger;
    
    public ResultFilter(ILogger<ResultFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is ObjectResult objectResult)
        {
            if (objectResult.Value is IResultBase result)
            {
                if (result.IsFailed)
                {
                    foreach (var error in result.Errors)
                    {
                        Log.Warning("Ошибка бизнес логики: {Message}", error.Message);
                    }

                    context.Result = new BadRequestObjectResult(new
                    {
                        Status = "Error",
                        Errors = result.Errors.Select(e => e.Message),
                        TimeStamp = DateTime.Now
                    });
                } 
                else if (objectResult.Value is Result<object> genericResult)
                {
                    context.Result = new OkObjectResult(genericResult.Value);
                }
            }
        }
        
    }
}