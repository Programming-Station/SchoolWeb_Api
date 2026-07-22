using Microsoft.AspNetCore.Mvc;
using School.Infrastructure.Interfaces;
using School.Services.Interfaces;
using School_API.Common.Interface;
using School_DTOs.Communication;

namespace School_API.Controllers.Communication
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NotificationController : BaseController
    {
        private readonly INotificationLogService _svc;
        private readonly ITenantService _tenant;

        public NotificationController(INotificationLogService svc, ITenantService tenant, ICurrentUserService cur) : base(cur)
        {
            _svc = svc;
            _tenant = tenant;
        }

        private int SchoolId => _tenant.GetTenantId() ?? 1;

        [HttpGet]
        public async Task<IActionResult> GetNotifications([FromQuery] NotificationLogFilterDto filter)
        {
            var r = await _svc.GetNotificationsAsync(UserId, filter, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> GetUnreadCount()
        {
            var r = await _svc.GetUnreadCountAsync(UserId, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDto dto)
        {
            var r = await _svc.CreateNotificationAsync(dto, UserId, SchoolId);
            return StatusCode((int)r.StatusCode, r);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var r = await _svc.MarkAsReadAsync(id, UserId, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var r = await _svc.MarkAllAsReadAsync(UserId, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var r = await _svc.DeleteNotificationAsync(id, UserId, SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }
    }
}
