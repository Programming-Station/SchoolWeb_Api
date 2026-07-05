using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using School.Infrastructure.Interfaces;
using School_API.Attributes;
using System.Linq;
using System.Threading.Tasks;

namespace School_API.Filters
{
    public class PermissionFilter : IAsyncAuthorizationFilter
    {
        private readonly IPermissionService _permissionService;

        public PermissionFilter(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var endpoint = context.ActionDescriptor.EndpointMetadata;
            var permissionAttribute = endpoint.OfType<HasPermissionAttribute>().FirstOrDefault();

            if (permissionAttribute != null)
            {
                bool hasPermission = await _permissionService.HasModulePermissionAsync(permissionAttribute.ModuleName);
                if (!hasPermission)
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }
}
