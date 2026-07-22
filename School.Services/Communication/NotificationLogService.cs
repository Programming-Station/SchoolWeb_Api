using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using School.Domain.Communication;
using School.Infrastructure;
using School.Services.Interfaces;
using School_DTOs;
using School_DTOs.Communication;

namespace School.Services.Communication
{
    public class NotificationLogService : INotificationLogService
    {
        private readonly SchoolDbContext _context;
        private readonly IMapper _mapper;

        public NotificationLogService(SchoolDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<APIResponse<List<NotificationLogDto>>> GetNotificationsAsync(string userId, NotificationLogFilterDto filter, int schoolId)
        {
            var query = _context.NotificationLogs
                .Where(n => n.RecipientUserId == userId && n.SchoolRegistrationId == schoolId && !n.IsDeleted);

            if (filter.IsRead.HasValue)
                query = query.Where(n => n.IsRead == filter.IsRead.Value);
            if (!string.IsNullOrEmpty(filter.Category))
                query = query.Where(n => n.Category == filter.Category);
            if (!string.IsNullOrEmpty(filter.NotificationType))
                query = query.Where(n => n.NotificationType == filter.NotificationType);

            // Exclude expired notifications
            query = query.Where(n => n.ExpiryDate == null || n.ExpiryDate > DateTime.UtcNow);

            var items = await query.OrderByDescending(n => n.CreatedDate).Take(100).ToListAsync();
            return new APIResponse<List<NotificationLogDto>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = _mapper.Map<List<NotificationLogDto>>(items)
            };
        }

        public async Task<APIResponse<NotificationCountDto>> GetUnreadCountAsync(string userId, int schoolId)
        {
            var query = _context.NotificationLogs
                .Where(n => n.RecipientUserId == userId && n.SchoolRegistrationId == schoolId && !n.IsDeleted && !n.IsRead)
                .Where(n => n.ExpiryDate == null || n.ExpiryDate > DateTime.UtcNow);

            var total = await query.CountAsync();
            var feeCount = await query.CountAsync(n => n.Category == "Fee");
            var attendanceCount = await query.CountAsync(n => n.Category == "Attendance");

            return new APIResponse<NotificationCountDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = new NotificationCountDto
                {
                    TotalUnread = total,
                    FeeNotifications = feeCount,
                    AttendanceNotifications = attendanceCount,
                    GeneralNotifications = total - feeCount - attendanceCount
                }
            };
        }

        public async Task<APIResponse<NotificationLogDto>> CreateNotificationAsync(CreateNotificationDto dto, string senderUserId, int schoolId)
        {
            var entity = _mapper.Map<NotificationLog>(dto);
            entity.SchoolRegistrationId = schoolId;
            entity.SenderUserId = senderUserId;
            entity.IsRead = false;
            entity.CreatedBy = senderUserId;
            entity.CreatedDate = DateTime.UtcNow;

            _context.NotificationLogs.Add(entity);
            await _context.SaveChangesAsync();

            return new APIResponse<NotificationLogDto> { Success = true, StatusCode = HttpStatusCode.Created, Data = _mapper.Map<NotificationLogDto>(entity) };
        }

        public async Task<APIResponse<bool>> MarkAsReadAsync(int id, string userId, int schoolId)
        {
            var entity = await _context.NotificationLogs
                .FirstOrDefaultAsync(n => n.Id == id && n.RecipientUserId == userId && n.SchoolRegistrationId == schoolId);

            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Notification not found" };

            entity.IsRead = true;
            entity.ReadDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Data = true };
        }

        public async Task<APIResponse<bool>> MarkAllAsReadAsync(string userId, int schoolId)
        {
            var unread = await _context.NotificationLogs
                .Where(n => n.RecipientUserId == userId && n.SchoolRegistrationId == schoolId && !n.IsRead && !n.IsDeleted)
                .ToListAsync();

            foreach (var n in unread)
            {
                n.IsRead = true;
                n.ReadDate = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();

            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Data = true, Message = $"{unread.Count} notifications marked as read" };
        }

        public async Task<APIResponse<bool>> DeleteNotificationAsync(int id, string userId, int schoolId)
        {
            var entity = await _context.NotificationLogs
                .FirstOrDefaultAsync(n => n.Id == id && n.RecipientUserId == userId && n.SchoolRegistrationId == schoolId);

            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Notification not found" };

            entity.IsDeleted = true;
            await _context.SaveChangesAsync();

            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Data = true };
        }
    }
}
