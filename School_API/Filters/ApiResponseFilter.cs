using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using School_DTOs;
using System.Net;
using System.Threading.Tasks;

namespace School_API.Filters
{
    public class ApiResponseFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is ObjectResult objectResult)
            {
                var valueType = objectResult.Value?.GetType();
                var isAlreadyApiResponse = valueType != null && 
                    (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(APIResponse<>) ||
                     valueType == typeof(APIResponse<object>));

                if (!isAlreadyApiResponse)
                {
                    var statusCode = objectResult.StatusCode ?? (int)HttpStatusCode.OK;
                    var message = "Success";
                    object? data = objectResult.Value;

                    // Extract message if returning anonymous type e.g. new { message = "..." }
                    if (objectResult.Value != null && valueType != null)
                    {
                        var messageProp = valueType.GetProperty("message");
                        if (messageProp != null)
                        {
                            message = messageProp.GetValue(objectResult.Value)?.ToString() ?? "Success";
                        }
                    }

                    var wrappedResponse = new APIResponse<object>
                    {
                        Success = statusCode >= 200 && statusCode < 300,
                        StatusCode = (HttpStatusCode)statusCode,
                        Message = message,
                        Data = data
                    };

                    objectResult.Value = wrappedResponse;
                }
            }
            else if (context.Result is StatusCodeResult statusCodeResult)
            {
                var statusCode = statusCodeResult.StatusCode;
                var wrappedResponse = new APIResponse<object>
                {
                    Success = statusCode >= 200 && statusCode < 300,
                    StatusCode = (HttpStatusCode)statusCode,
                    Message = statusCode >= 200 && statusCode < 300 ? "Success" : "Error",
                    Data = null
                };

                context.Result = new ObjectResult(wrappedResponse)
                {
                    StatusCode = statusCode
                };
            }

            await next();
        }
    }
}
