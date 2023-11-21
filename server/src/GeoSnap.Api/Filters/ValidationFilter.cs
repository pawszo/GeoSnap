using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GeoSnap.Api.Filters;
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ValidationFilterAttribute : ActionFilterAttribute, IAsyncExceptionFilter
{
    public Task OnExceptionAsync(ExceptionContext context)
    {
        if (context.Exception is Domain.Exceptions.ValidationException)
        {
            context.Result = new BadRequestObjectResult(context.Exception.Message);
            context.ExceptionHandled = true;
        }

        return Task.CompletedTask;
    }
}
