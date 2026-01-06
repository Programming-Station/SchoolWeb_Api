using School_DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using System.Net;

namespace School_API
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .SelectMany(x => x.Value?.Errors.Select(e => e.ErrorMessage) ?? Enumerable.Empty<string>())
                    .ToArray();

                var result = new APIResponse
                {
                    Message = "One or more validation errors occurred.",
                    StatusCode = HttpStatusCode.BadRequest,
                    Error = new APIException(string.Join(", ", errors)),
                    RequestId = context.HttpContext.TraceIdentifier
                };

                context.Result = new BadRequestObjectResult(result);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}

