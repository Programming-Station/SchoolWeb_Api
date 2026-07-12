using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.Domain.Communication;
using School.Infrastructure;
using School.Services.Interfaces;
using School_DTOs;
using School_DTOs.Communication;

namespace School.Services.Communication
{
    public class CommunicationService : ICommunicationService
    {
        private readonly SchoolDbContext _context;

        public CommunicationService(SchoolDbContext context)
        {
            _context = context;
        }

        private int GetCurrentSchoolId()
        {
            if (_context.CurrentTenantId.HasValue)
                return _context.CurrentTenantId.Value;
            var school = _context.SchoolRegistrations.IgnoreQueryFilters().FirstOrDefault();
            return school?.Id ?? 1;
        }

        // ════════════════════════════════════════════════════════════════════════
        // NOTICE BOARD
        // ════════════════════════════════════════════════════════════════════════

        public async Task<APIResponse<IEnumerable<NoticeBoardDto>>> GetNoticesAsync(NoticeBoardFilterDto? filter = null)
        {
            var q = _context.NoticeBoards.Where(n => !n.IsDeleted).AsQueryable();

            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.TargetAudience) && filter.TargetAudience != "All")
                    q = q.Where(n => n.TargetAudience == filter.TargetAudience);
                if (filter.IsPinned.HasValue)
                    q = q.Where(n => n.IsPinned == filter.IsPinned.Value);
                if (filter.FromDate.HasValue)
                    q = q.Where(n => n.PublishedDate >= filter.FromDate.Value);
                if (filter.ToDate.HasValue)
                    q = q.Where(n => n.PublishedDate <= filter.ToDate.Value);
            }

            var list = await q.OrderByDescending(n => n.IsPinned).ThenByDescending(n => n.PublishedDate).ToListAsync();
            return new APIResponse<IEnumerable<NoticeBoardDto>>
            {
                Success = true, StatusCode = HttpStatusCode.OK,
                Data = list.Select(MapNotice)
            };
        }

        public async Task<APIResponse<NoticeBoardDto>> GetNoticeByIdAsync(int id)
        {
            var n = await _context.NoticeBoards.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (n == null)
                return new APIResponse<NoticeBoardDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Notice not found" };
            return new APIResponse<NoticeBoardDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = MapNotice(n) };
        }

        public async Task<APIResponse<NoticeBoardDto>> CreateNoticeAsync(NoticeBoardDto dto, string userName)
        {
            var entity = new NoticeBoard
            {
                Title = dto.Title, Content = dto.Content,
                TargetAudience = dto.TargetAudience,
                PublishedDate = dto.PublishedDate == default ? DateTime.UtcNow : dto.PublishedDate,
                ExpiryDate = dto.ExpiryDate, IsPinned = dto.IsPinned,
                SchoolRegistrationId = GetCurrentSchoolId(), CreatedBy = userName
            };
            _context.NoticeBoards.Add(entity);
            await _context.SaveChangesAsync();
            dto.Id = entity.Id; dto.CreatedBy = userName; dto.CreatedDate = entity.CreatedDate;
            return new APIResponse<NoticeBoardDto> { Success = true, StatusCode = HttpStatusCode.Created, Message = "Notice published", Data = dto };
        }

        public async Task<APIResponse<NoticeBoardDto>> UpdateNoticeAsync(int id, NoticeBoardDto dto, string userName)
        {
            var entity = await _context.NoticeBoards.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (entity == null)
                return new APIResponse<NoticeBoardDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Notice not found" };

            entity.Title = dto.Title; entity.Content = dto.Content;
            entity.TargetAudience = dto.TargetAudience;
            entity.PublishedDate = dto.PublishedDate == default ? entity.PublishedDate : dto.PublishedDate;
            entity.ExpiryDate = dto.ExpiryDate; entity.IsPinned = dto.IsPinned;
            entity.UpdatedBy = userName; entity.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return new APIResponse<NoticeBoardDto> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Notice updated", Data = MapNotice(entity) };
        }

        public async Task<APIResponse<bool>> DeleteNoticeAsync(int id)
        {
            var entity = await _context.NoticeBoards.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Notice not found" };
            entity.IsDeleted = true;
            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Notice deleted", Data = true };
        }

        private static NoticeBoardDto MapNotice(NoticeBoard n) => new()
        {
            Id = n.Id, Title = n.Title, Content = n.Content,
            TargetAudience = n.TargetAudience, PublishedDate = n.PublishedDate,
            ExpiryDate = n.ExpiryDate, IsPinned = n.IsPinned,
            CreatedBy = n.CreatedBy, CreatedDate = n.CreatedDate,
            UpdatedBy = n.UpdatedBy, UpdatedDate = n.UpdatedDate
        };

        // ════════════════════════════════════════════════════════════════════════
        // CIRCULARS
        // ════════════════════════════════════════════════════════════════════════

        public async Task<APIResponse<IEnumerable<CircularDto>>> GetCircularsAsync(CircularFilterDto? filter = null)
        {
            var q = _context.Circulars.Where(c => !c.IsDeleted).AsQueryable();

            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.TargetAudience) && filter.TargetAudience != "All")
                    q = q.Where(c => c.TargetAudience == filter.TargetAudience);
                if (!string.IsNullOrWhiteSpace(filter.Keyword))
                    q = q.Where(c => c.Subject.Contains(filter.Keyword) || c.Content.Contains(filter.Keyword));
                if (filter.FromDate.HasValue)
                    q = q.Where(c => c.PublishDate >= filter.FromDate.Value);
                if (filter.ToDate.HasValue)
                    q = q.Where(c => c.PublishDate <= filter.ToDate.Value);
            }

            var list = await q.OrderByDescending(c => c.PublishDate).ToListAsync();
            return new APIResponse<IEnumerable<CircularDto>>
            {
                Success = true, StatusCode = HttpStatusCode.OK,
                Data = list.Select(MapCircular)
            };
        }

        public async Task<APIResponse<CircularDto>> GetCircularByIdAsync(int id)
        {
            var c = await _context.Circulars.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (c == null)
                return new APIResponse<CircularDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Circular not found" };
            return new APIResponse<CircularDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = MapCircular(c) };
        }

        public async Task<APIResponse<CircularDto>> CreateCircularAsync(CircularDto dto, string userName)
        {
            var entity = new Circular
            {
                CircularNo = dto.CircularNo, Subject = dto.Subject, Content = dto.Content,
                AttachmentPath = dto.AttachmentPath, TargetAudience = dto.TargetAudience,
                PublishDate = dto.PublishDate == default ? DateTime.UtcNow : dto.PublishDate,
                SchoolRegistrationId = GetCurrentSchoolId(), CreatedBy = userName
            };
            _context.Circulars.Add(entity);
            await _context.SaveChangesAsync();
            dto.Id = entity.Id; dto.CreatedBy = userName; dto.CreatedDate = entity.CreatedDate;
            return new APIResponse<CircularDto> { Success = true, StatusCode = HttpStatusCode.Created, Message = "Circular issued", Data = dto };
        }

        public async Task<APIResponse<CircularDto>> UpdateCircularAsync(int id, CircularDto dto, string userName)
        {
            var entity = await _context.Circulars.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (entity == null)
                return new APIResponse<CircularDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Circular not found" };

            entity.CircularNo = dto.CircularNo; entity.Subject = dto.Subject; entity.Content = dto.Content;
            entity.AttachmentPath = dto.AttachmentPath; entity.TargetAudience = dto.TargetAudience;
            entity.PublishDate = dto.PublishDate == default ? entity.PublishDate : dto.PublishDate;
            entity.UpdatedBy = userName; entity.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return new APIResponse<CircularDto> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Circular updated", Data = MapCircular(entity) };
        }

        public async Task<APIResponse<bool>> DeleteCircularAsync(int id)
        {
            var entity = await _context.Circulars.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Circular not found" };
            entity.IsDeleted = true;
            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Circular deleted", Data = true };
        }

        private static CircularDto MapCircular(Circular c) => new()
        {
            Id = c.Id, CircularNo = c.CircularNo, Subject = c.Subject, Content = c.Content,
            AttachmentPath = c.AttachmentPath, TargetAudience = c.TargetAudience, PublishDate = c.PublishDate,
            CreatedBy = c.CreatedBy, CreatedDate = c.CreatedDate,
            UpdatedBy = c.UpdatedBy, UpdatedDate = c.UpdatedDate
        };

        // ════════════════════════════════════════════════════════════════════════
        // SMS GATEWAY
        // ════════════════════════════════════════════════════════════════════════

        public async Task<APIResponse<IEnumerable<SmsLogDto>>> GetSmsLogsAsync(GatewayLogFilterDto? filter = null)
        {
            var q = _context.SmsLogs.Where(l => !l.IsDeleted).AsQueryable();

            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.RecipientNo))
                    q = q.Where(l => l.RecipientNo.Contains(filter.RecipientNo));
                if (!string.IsNullOrWhiteSpace(filter.Status))
                    q = q.Where(l => l.SentStatus == filter.Status);
                if (filter.FromDate.HasValue)
                    q = q.Where(l => l.SentDate >= filter.FromDate.Value);
                if (filter.ToDate.HasValue)
                    q = q.Where(l => l.SentDate <= filter.ToDate.Value);
            }

            var list = await q.OrderByDescending(l => l.SentDate).ToListAsync();
            return new APIResponse<IEnumerable<SmsLogDto>>
            {
                Success = true, StatusCode = HttpStatusCode.OK,
                Data = list.Select(l => new SmsLogDto
                {
                    Id = l.Id, RecipientNo = l.RecipientNo, Message = l.Message,
                    SentStatus = l.SentStatus, SentDate = l.SentDate, ProviderResponse = l.ProviderResponse
                })
            };
        }

        public async Task<APIResponse<SmsLogDto>> GetSmsLogByIdAsync(int id)
        {
            var l = await _context.SmsLogs.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (l == null)
                return new APIResponse<SmsLogDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "SMS log not found" };
            return new APIResponse<SmsLogDto>
            {
                Success = true, StatusCode = HttpStatusCode.OK,
                Data = new SmsLogDto { Id = l.Id, RecipientNo = l.RecipientNo, Message = l.Message, SentStatus = l.SentStatus, SentDate = l.SentDate, ProviderResponse = l.ProviderResponse }
            };
        }

        public async Task<APIResponse<bool>> SendSmsAsync(SmsLogDto dto, string userName)
        {
            var entity = new SmsLog
            {
                RecipientNo = dto.RecipientNo, Message = dto.Message,
                SentStatus = "Sent", SentDate = DateTime.UtcNow,
                ProviderResponse = "REF-" + Guid.NewGuid().ToString("N")[..8],
                SchoolRegistrationId = GetCurrentSchoolId(), CreatedBy = userName
            };
            _context.SmsLogs.Add(entity);
            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Message = "SMS dispatched", Data = true };
        }

        // ════════════════════════════════════════════════════════════════════════
        // WHATSAPP GATEWAY
        // ════════════════════════════════════════════════════════════════════════

        public async Task<APIResponse<IEnumerable<WhatsAppLogDto>>> GetWhatsAppLogsAsync(GatewayLogFilterDto? filter = null)
        {
            var q = _context.WhatsAppLogs.Where(l => !l.IsDeleted).AsQueryable();

            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.RecipientNo))
                    q = q.Where(l => l.RecipientPhone.Contains(filter.RecipientNo));
                if (!string.IsNullOrWhiteSpace(filter.Status))
                    q = q.Where(l => l.Status == filter.Status);
                if (filter.FromDate.HasValue)
                    q = q.Where(l => l.SentDate >= filter.FromDate.Value);
                if (filter.ToDate.HasValue)
                    q = q.Where(l => l.SentDate <= filter.ToDate.Value);
            }

            var list = await q.OrderByDescending(l => l.SentDate).ToListAsync();
            return new APIResponse<IEnumerable<WhatsAppLogDto>>
            {
                Success = true, StatusCode = HttpStatusCode.OK,
                Data = list.Select(l => new WhatsAppLogDto { Id = l.Id, RecipientPhone = l.RecipientPhone, Message = l.Message, Status = l.Status, SentDate = l.SentDate })
            };
        }

        public async Task<APIResponse<WhatsAppLogDto>> GetWhatsAppLogByIdAsync(int id)
        {
            var l = await _context.WhatsAppLogs.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (l == null)
                return new APIResponse<WhatsAppLogDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "WhatsApp log not found" };
            return new APIResponse<WhatsAppLogDto>
            {
                Success = true, StatusCode = HttpStatusCode.OK,
                Data = new WhatsAppLogDto { Id = l.Id, RecipientPhone = l.RecipientPhone, Message = l.Message, Status = l.Status, SentDate = l.SentDate }
            };
        }

        public async Task<APIResponse<bool>> SendWhatsAppAsync(WhatsAppLogDto dto, string userName)
        {
            var entity = new WhatsAppLog
            {
                RecipientPhone = dto.RecipientPhone, Message = dto.Message,
                Status = "Delivered", SentDate = DateTime.UtcNow,
                SchoolRegistrationId = GetCurrentSchoolId(), CreatedBy = userName
            };
            _context.WhatsAppLogs.Add(entity);
            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Message = "WhatsApp message sent", Data = true };
        }

        // ════════════════════════════════════════════════════════════════════════
        // PUSH NOTIFICATIONS
        // ════════════════════════════════════════════════════════════════════════

        public async Task<APIResponse<IEnumerable<PushNotificationDto>>> GetPushNotificationsAsync(string userId, GatewayLogFilterDto? filter = null)
        {
            var q = _context.PushNotifications.Where(p => p.RecipientUserId == userId && !p.IsDeleted).AsQueryable();

            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.Status) && filter.Status == "Unread")
                    q = q.Where(p => !p.IsRead);
                if (filter.FromDate.HasValue)
                    q = q.Where(p => p.SentDate >= filter.FromDate.Value);
                if (filter.ToDate.HasValue)
                    q = q.Where(p => p.SentDate <= filter.ToDate.Value);
            }

            var list = await q.OrderByDescending(p => p.SentDate).ToListAsync();
            return new APIResponse<IEnumerable<PushNotificationDto>>
            {
                Success = true, StatusCode = HttpStatusCode.OK,
                Data = list.Select(p => new PushNotificationDto { Id = p.Id, RecipientUserId = p.RecipientUserId, Title = p.Title, Body = p.Body, IsRead = p.IsRead, SentDate = p.SentDate })
            };
        }

        public async Task<APIResponse<PushNotificationDto>> GetPushNotificationByIdAsync(int id)
        {
            var p = await _context.PushNotifications.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (p == null)
                return new APIResponse<PushNotificationDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Notification not found" };
            return new APIResponse<PushNotificationDto>
            {
                Success = true, StatusCode = HttpStatusCode.OK,
                Data = new PushNotificationDto { Id = p.Id, RecipientUserId = p.RecipientUserId, Title = p.Title, Body = p.Body, IsRead = p.IsRead, SentDate = p.SentDate }
            };
        }

        public async Task<APIResponse<bool>> SendPushNotificationAsync(PushNotificationDto dto, string userName)
        {
            var entity = new PushNotification
            {
                RecipientUserId = dto.RecipientUserId, Title = dto.Title, Body = dto.Body,
                IsRead = false, SentDate = DateTime.UtcNow,
                SchoolRegistrationId = GetCurrentSchoolId(), CreatedBy = userName
            };
            _context.PushNotifications.Add(entity);
            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Push notification logged", Data = true };
        }

        public async Task<APIResponse<bool>> MarkPushNotificationReadAsync(int id, string userName)
        {
            var entity = await _context.PushNotifications.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Notification not found" };
            entity.IsRead = true;
            entity.UpdatedBy = userName; entity.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Marked as read", Data = true };
        }

        // ════════════════════════════════════════════════════════════════════════
        // PARENT-TEACHER CHAT
        // ════════════════════════════════════════════════════════════════════════

        public async Task<APIResponse<IEnumerable<ParentTeacherChatDto>>> GetChatHistoryAsync(string senderId, string receiverId)
        {
            var chats = await _context.ParentTeacherChats
                .Where(c => !c.IsDeleted &&
                    ((c.SenderUserId == senderId && c.ReceiverUserId == receiverId) ||
                     (c.SenderUserId == receiverId && c.ReceiverUserId == senderId)))
                .OrderBy(c => c.SentTime)
                .ToListAsync();

            var userIds = chats.Select(c => c.SenderUserId).Concat(chats.Select(c => c.ReceiverUserId)).Distinct().ToList();
            var users = await _context.Users.IgnoreQueryFilters()
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.UserName ?? "User");

            return new APIResponse<IEnumerable<ParentTeacherChatDto>>
            {
                Success = true, StatusCode = HttpStatusCode.OK,
                Data = chats.Select(c => new ParentTeacherChatDto
                {
                    Id = c.Id, SenderUserId = c.SenderUserId,
                    SenderUserName = users.GetValueOrDefault(c.SenderUserId, "User"),
                    ReceiverUserId = c.ReceiverUserId,
                    ReceiverUserName = users.GetValueOrDefault(c.ReceiverUserId, "User"),
                    MessageContent = c.MessageContent, SentTime = c.SentTime, IsRead = c.IsRead
                })
            };
        }

        public async Task<APIResponse<ParentTeacherChatDto>> GetChatMessageByIdAsync(int id)
        {
            var c = await _context.ParentTeacherChats.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (c == null)
                return new APIResponse<ParentTeacherChatDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Message not found" };
            return new APIResponse<ParentTeacherChatDto>
            {
                Success = true, StatusCode = HttpStatusCode.OK,
                Data = new ParentTeacherChatDto { Id = c.Id, SenderUserId = c.SenderUserId, ReceiverUserId = c.ReceiverUserId, MessageContent = c.MessageContent, SentTime = c.SentTime, IsRead = c.IsRead }
            };
        }

        public async Task<APIResponse<bool>> SendChatMessageAsync(ParentTeacherChatDto dto, string userName)
        {
            var entity = new ParentTeacherChat
            {
                SenderUserId = dto.SenderUserId, ReceiverUserId = dto.ReceiverUserId,
                MessageContent = dto.MessageContent, SentTime = DateTime.UtcNow, IsRead = false,
                SchoolRegistrationId = GetCurrentSchoolId(), CreatedBy = userName
            };
            _context.ParentTeacherChats.Add(entity);
            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Message sent", Data = true };
        }

        public async Task<APIResponse<bool>> MarkChatMessageReadAsync(int id, string userName)
        {
            var entity = await _context.ParentTeacherChats.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Message not found" };
            entity.IsRead = true;
            entity.UpdatedBy = userName; entity.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Marked as read", Data = true };
        }

        public async Task<APIResponse<bool>> DeleteChatMessageAsync(int id)
        {
            var entity = await _context.ParentTeacherChats.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Message not found" };
            entity.IsDeleted = true;
            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Message deleted", Data = true };
        }

        // ════════════════════════════════════════════════════════════════════════
        // FEEDBACK & SURVEYS
        // ════════════════════════════════════════════════════════════════════════

        public async Task<APIResponse<IEnumerable<FeedbackSurveyDto>>> GetSurveysAsync(SurveyFilterDto? filter = null)
        {
            var q = _context.FeedbackSurveys.Include(s => s.Questions).Where(s => !s.IsDeleted).AsQueryable();

            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.TargetAudience) && filter.TargetAudience != "All")
                    q = q.Where(s => s.TargetAudience == filter.TargetAudience);
                if (filter.IsActive.HasValue)
                    q = q.Where(s => s.IsActive == filter.IsActive.Value);
                if (!string.IsNullOrWhiteSpace(filter.Keyword))
                    q = q.Where(s => s.Title.Contains(filter.Keyword));
            }

            var list = await q.OrderByDescending(s => s.CreatedDate).ToListAsync();
            return new APIResponse<IEnumerable<FeedbackSurveyDto>>
            {
                Success = true, StatusCode = HttpStatusCode.OK,
                Data = list.Select(MapSurvey)
            };
        }

        public async Task<APIResponse<FeedbackSurveyDto>> GetSurveyByIdAsync(int id)
        {
            var s = await _context.FeedbackSurveys.Include(x => x.Questions).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (s == null)
                return new APIResponse<FeedbackSurveyDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Survey not found" };
            return new APIResponse<FeedbackSurveyDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = MapSurvey(s) };
        }

        public async Task<APIResponse<FeedbackSurveyDto>> CreateSurveyAsync(FeedbackSurveyDto dto, string userName)
        {
            var schoolId = GetCurrentSchoolId();
            var entity = new FeedbackSurvey
            {
                Title = dto.Title, Description = dto.Description,
                TargetAudience = dto.TargetAudience, IsActive = dto.IsActive,
                SchoolRegistrationId = schoolId, CreatedBy = userName
            };
            foreach (var q in dto.Questions)
            {
                entity.Questions.Add(new SurveyQuestion
                {
                    QuestionText = q.QuestionText, QuestionType = q.QuestionType,
                    OptionsJson = q.OptionsJson, SchoolRegistrationId = schoolId, CreatedBy = userName
                });
            }
            _context.FeedbackSurveys.Add(entity);
            await _context.SaveChangesAsync();
            dto.Id = entity.Id; dto.CreatedBy = userName;
            return new APIResponse<FeedbackSurveyDto> { Success = true, StatusCode = HttpStatusCode.Created, Message = "Survey created", Data = dto };
        }

        public async Task<APIResponse<FeedbackSurveyDto>> UpdateSurveyAsync(int id, FeedbackSurveyDto dto, string userName)
        {
            var entity = await _context.FeedbackSurveys.Include(s => s.Questions).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (entity == null)
                return new APIResponse<FeedbackSurveyDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Survey not found" };

            entity.Title = dto.Title; entity.Description = dto.Description;
            entity.TargetAudience = dto.TargetAudience; entity.IsActive = dto.IsActive;
            entity.UpdatedBy = userName; entity.UpdatedDate = DateTime.UtcNow;

            // Rebuild questions: soft-delete removed ones, add new ones
            var schoolId = GetCurrentSchoolId();
            foreach (var existing in entity.Questions.Where(q => !q.IsDeleted).ToList())
            {
                if (!dto.Questions.Any(q => q.Id == existing.Id))
                    existing.IsDeleted = true;
            }
            foreach (var q in dto.Questions.Where(q => q.Id == 0))
            {
                entity.Questions.Add(new SurveyQuestion
                {
                    QuestionText = q.QuestionText, QuestionType = q.QuestionType,
                    OptionsJson = q.OptionsJson, SchoolRegistrationId = schoolId, CreatedBy = userName
                });
            }

            await _context.SaveChangesAsync();
            return new APIResponse<FeedbackSurveyDto> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Survey updated", Data = MapSurvey(entity) };
        }

        public async Task<APIResponse<bool>> DeleteSurveyAsync(int id)
        {
            var entity = await _context.FeedbackSurveys.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Survey not found" };
            entity.IsDeleted = true;
            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Survey deleted", Data = true };
        }

        public async Task<APIResponse<bool>> SubmitSurveyResponseAsync(SurveyResponseDto dto, string userName)
        {
            var entity = new SurveyResponse
            {
                SurveyId = dto.SurveyId, RespondentUserId = dto.RespondentUserId,
                AnswersJson = dto.AnswersJson, SubmittedDate = DateTime.UtcNow,
                SchoolRegistrationId = GetCurrentSchoolId(), CreatedBy = userName
            };
            _context.SurveyResponses.Add(entity);
            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Response recorded", Data = true };
        }

        public async Task<APIResponse<IEnumerable<SurveyResponseDto>>> GetSurveyResponsesAsync(int surveyId)
        {
            var responses = await _context.SurveyResponses
                .Where(r => r.SurveyId == surveyId && !r.IsDeleted)
                .OrderByDescending(r => r.SubmittedDate)
                .ToListAsync();

            return new APIResponse<IEnumerable<SurveyResponseDto>>
            {
                Success = true, StatusCode = HttpStatusCode.OK,
                Data = responses.Select(r => new SurveyResponseDto
                {
                    Id = r.Id, SurveyId = r.SurveyId, RespondentUserId = r.RespondentUserId,
                    AnswersJson = r.AnswersJson, SubmittedDate = r.SubmittedDate
                })
            };
        }

        private static FeedbackSurveyDto MapSurvey(FeedbackSurvey s) => new()
        {
            Id = s.Id, Title = s.Title, Description = s.Description,
            TargetAudience = s.TargetAudience, CreatedDate = s.CreatedDate ?? DateTime.UtcNow,
            IsActive = s.IsActive, CreatedBy = s.CreatedBy, UpdatedBy = s.UpdatedBy, UpdatedDate = s.UpdatedDate,
            Questions = s.Questions.Where(q => !q.IsDeleted).Select(q => new SurveyQuestionDto
            {
                Id = q.Id, SurveyId = q.SurveyId, QuestionText = q.QuestionText,
                QuestionType = q.QuestionType, OptionsJson = q.OptionsJson
            }).ToList()
        };
    }
}
