using School_DTOs;
using School.Models.Configuration;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json; 

namespace School_API.Middleware
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly AppSettings? _appSettings;

        public CustomExceptionHandlerMiddleware(
            RequestDelegate next, 
            ILogger<CustomExceptionHandlerMiddleware> logger,
            IWebHostEnvironment environment,
            IOptions<AppSettings>? appSettings = null)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
            _appSettings = appSettings?.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Swagger authentication check
                if (context.Request.Path.StartsWithSegments("/swagger"))
                {
                    string? authHeader = context.Request.Headers["Authorization"];
                    if (authHeader != null && authHeader.StartsWith("Basic "))
                    {
                        var encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();
                        if (!string.IsNullOrEmpty(encodedUsernamePassword))
                        {
                            try
                            {
                                var decodedUsernamePassword = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));
                                var credentials = decodedUsernamePassword.Split(':', 2);
                                if (credentials.Length == 2)
                                {
                                    var username = credentials[0];
                                    var password = credentials[1];

                                    if (IsAuthorized(username, password))
                                    {
                                        await _next(context);
                                        return;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning(ex, "Failed to decode Swagger authentication");
                            }
                        }
                    }

                    context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Swagger\"";
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }
                else
                {
                    await _next(context);
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = new APIResponse();
            var code = HttpStatusCode.InternalServerError;
            var requestId = context.TraceIdentifier;
            var isDevelopment = _environment.IsDevelopment();

            // Determine HTTP status code and message based on exception type
            switch (exception)
            {
                case UnauthorizedAccessException:
                    code = HttpStatusCode.Unauthorized;
                    response.Message = exception.Message ?? "Unauthorized access";
                    break;

                case ArgumentException argEx:
                    code = HttpStatusCode.BadRequest;
                    response.Message = argEx.Message ?? "Invalid argument";
                    break; 

                case KeyNotFoundException:
                    code = HttpStatusCode.NotFound;
                    response.Message = exception.Message ?? "Resource not found";
                    break;

                case FileNotFoundException:
                    code = HttpStatusCode.NotFound;
                    response.Message = exception.Message ?? "File not found";
                    break;

                case InvalidOperationException invalidOpEx:
                    code = HttpStatusCode.BadRequest;
                    response.Message = invalidOpEx.Message ?? "Invalid operation";
                    break;

                case NotSupportedException:
                    code = HttpStatusCode.MethodNotAllowed;
                    response.Message = exception.Message ?? "Operation not supported";
                    break;

                case TimeoutException:
                    code = HttpStatusCode.RequestTimeout;
                    response.Message = exception.Message ?? "Request timeout";
                    break;

                case Microsoft.EntityFrameworkCore.DbUpdateException dbEx:
                    code = HttpStatusCode.InternalServerError;
                    response.Message = "Database operation failed";
                    _logger.LogError(dbEx, "Database error occurred. RequestId: {RequestId}", requestId);
                    break;

                case JsonException jsonEx:
                    code = HttpStatusCode.BadRequest;
                    response.Message = "Invalid JSON format";
                    break;

                default:
                    code = HttpStatusCode.InternalServerError;
                    response.Message = "An error occurred while processing your request";
                    _logger.LogError(exception, "Unhandled exception occurred. RequestId: {RequestId}", requestId);
                    break;
            }

            // Build error response
            response.StatusCode = code;
            response.Success = false;
            response.RequestId = requestId;
            //response.Timestamp = DateTime.UtcNow;

            // Create error details
            var errorMessage = exception.Message;
            
            // Include inner exception message if available
            if (exception.InnerException != null)
            {
                errorMessage += $" | Inner: {exception.InnerException.Message}";
            }

            response.Error = new APIException(errorMessage, exception);

            // Don't expose stack trace in production unless it's a known error type
            if (!isDevelopment && code == HttpStatusCode.InternalServerError)
            {
                response.Error.StackTrace = null;
                response.Message = "An internal server error occurred. Please try again later.";
            }

            // Ensure response hasn't started
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = (int)code;
                context.Response.ContentType = "application/json; charset=utf-8";

                // Ensure CORS headers are present if CORS is enabled
                // This is important for error responses to work with CORS
                if (!context.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
                {
                    var origin = context.Request.Headers["Origin"].ToString();
                    if (!string.IsNullOrEmpty(origin))
                    {
                        // Check if origin is in allowed list, or allow if CORS is enabled and origin exists
                        if (_appSettings?.EnableCors == true)
                        {
                            var allowedOrigins = _appSettings.AllowedOrigins;
                            if (allowedOrigins != null && allowedOrigins.Contains(origin))
                            {
                                context.Response.Headers.Append("Access-Control-Allow-Origin", origin);
                                context.Response.Headers.Append("Access-Control-Allow-Credentials", "true");
                            }
                            else if (allowedOrigins == null || allowedOrigins.Count == 0)
                            {
                                // Fallback: allow origin if no restrictions
                                context.Response.Headers.Append("Access-Control-Allow-Origin", origin);
                            }
                        }
                        else
                        {
                            // If CORS is disabled, still allow for development
                            if (_environment.IsDevelopment())
                            {
                                context.Response.Headers.Append("Access-Control-Allow-Origin", origin);
                            }
                        }
                    }
                }

                // Serialize and write response
                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = false
                };

                var json = JsonSerializer.Serialize(response, jsonOptions);
                await context.Response.WriteAsync(json);
            }
        }

        private bool IsAuthorized(string username, string password)
        {
            var swaggerUsername = Environment.GetEnvironmentVariable("SWAGGER_USERNAME") ?? "admin";
            var swaggerPassword = Environment.GetEnvironmentVariable("SWAGGER_PASSWORD") ?? "admin123";

            return username.Equals(swaggerUsername, StringComparison.OrdinalIgnoreCase) 
                && password.Equals(swaggerPassword, StringComparison.Ordinal);
        }
    }

    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}

