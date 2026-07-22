using School_DTOs;
using School_DTOs.Communication;

namespace School.Services.Interfaces
{
    public interface INotificationLogService
    {
        Task<APIResponse<List<NotificationLogDto>>> GetNotificationsAsync(string userId, NotificationLogFilterDto filter, int schoolId);
        Task<APIResponse<NotificationCountDto>> GetUnreadCountAsync(string userId, int schoolId);
        Task<APIResponse<NotificationLogDto>> CreateNotificationAsync(CreateNotificationDto dto, string senderUserId, int schoolId);
        Task<APIResponse<bool>> MarkAsReadAsync(int id, string userId, int schoolId);
        Task<APIResponse<bool>> MarkAllAsReadAsync(string userId, int schoolId);
        Task<APIResponse<bool>> DeleteNotificationAsync(int id, string userId, int schoolId);
    }
}
