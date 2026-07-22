using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;
using School.Services.Interfaces;

namespace School_API.Filters
{
    public class AuditActionFilter : IAsyncActionFilter
    {
        private readonly IAuditService _auditService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditActionFilter(IAuditService auditService, IHttpContextAccessor httpContextAccessor)
        {
            _auditService = auditService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Execute the action
            var executedContext = await next();

            // Only log successful executions (can be expanded to log failures too)
            if (executedContext.Exception == null)
            {
                var httpContext = _httpContextAccessor.HttpContext;
                var userId = httpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
                var actionName = context.ActionDescriptor.DisplayName ?? "UnknownAction";

                var auditLog = new School.Domain.Entities.AuditLog
                {
                    UserId = userId,
                    Action = "Execute",
                    EntityName = actionName,
                    EntityId = null,
                    OldValues = null,
                    NewValues = null,
                    Timestamp = System.DateTime.UtcNow,
                    IPAddress = httpContext?.Connection?.RemoteIpAddress?.ToString()
                };

                await _auditService.LogAsync(auditLog);
            }
        }
    }
}
