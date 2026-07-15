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
        private readonly IMessageService _messageService;

        public CommunicationService(SchoolDbContext context, IMessageService messageService)
        {
            _context = context;
            _messageService = messageService;
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
                SchoolRegistrationId = GetCurrentSchoolId(), CreatedBy = userName, CreatedDate = DateTime.UtcNow
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
                SchoolRegistrationId = GetCurrentSchoolId(), CreatedBy = userName, CreatedDate = DateTime.UtcNow
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
            var isSent = await _messageService.SendSmsAsync(dto.RecipientNo, dto.Message);
            return new APIResponse<bool>
            {
                Success = isSent,
                StatusCode = isSent ? HttpStatusCode.OK : HttpStatusCode.InternalServerError,
                Message = isSent ? "SMS dispatched successfully" : "Failed to dispatch SMS",
                Data = isSent
            };
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
            var isSent = await _messageService.SendWhatsAppAsync(dto.RecipientPhone, dto.Message);
            return new APIResponse<bool>
            {
                Success = isSent,
                StatusCode = isSent ? HttpStatusCode.OK : HttpStatusCode.InternalServerError,
                Message = isSent ? "WhatsApp message dispatched successfully" : "Failed to dispatch WhatsApp message via gateway",
                Data = isSent
            };
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
                SchoolRegistrationId = GetCurrentSchoolId(), CreatedBy = userName, CreatedDate = DateTime.UtcNow
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
                SchoolRegistrationId = GetCurrentSchoolId(), CreatedBy = userName, CreatedDate = DateTime.UtcNow
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
                SchoolRegistrationId = schoolId, CreatedBy = userName, CreatedDate = DateTime.UtcNow
            };
            foreach (var q in dto.Questions)
            {
                entity.Questions.Add(new SurveyQuestion
                {
                    QuestionText = q.QuestionText, QuestionType = q.QuestionType,
                    OptionsJson = q.OptionsJson, SchoolRegistrationId = schoolId, CreatedBy = userName, CreatedDate = DateTime.UtcNow
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
                    OptionsJson = q.OptionsJson, SchoolRegistrationId = schoolId, CreatedBy = userName, CreatedDate = DateTime.UtcNow
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
                SchoolRegistrationId = GetCurrentSchoolId(), CreatedBy = userName, CreatedDate = DateTime.UtcNow
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

            var userIds = responses.Select(r => r.RespondentUserId).Distinct().ToList();
            var users = await _context.Users.IgnoreQueryFilters()
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.UserName ?? "User");

            return new APIResponse<IEnumerable<SurveyResponseDto>>
            {
                Success = true, StatusCode = HttpStatusCode.OK,
                Data = responses.Select(r => new SurveyResponseDto
                {
                    Id = r.Id, SurveyId = r.SurveyId, RespondentUserId = r.RespondentUserId,
                    RespondentName = users.GetValueOrDefault(r.RespondentUserId, "User"),
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


        // ════════════════════════════════════════════════════════════════════════
        // ANNOUNCEMENT MANAGEMENT
        // ════════════════════════════════════════════════════════════════════════

        public async Task<APIResponse<IEnumerable<AnnouncementDto>>> GetAnnouncementsAsync(AnnouncementFilterDto? filter = null)
        {
            var q = _context.Announcements.Where(a => !a.IsDeleted).AsQueryable();

            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.Scope) && filter.Scope != "All")
                    q = q.Where(a => a.Scope == filter.Scope);
                if (!string.IsNullOrWhiteSpace(filter.TargetReferenceId))
                    q = q.Where(a => a.TargetReferenceId == filter.TargetReferenceId);
                if (!string.IsNullOrWhiteSpace(filter.Priority))
                    q = q.Where(a => a.Priority == filter.Priority);
                if (!string.IsNullOrWhiteSpace(filter.Keyword))
                    q = q.Where(a => a.Title.Contains(filter.Keyword) || a.Content.Contains(filter.Keyword));
                if (filter.FromDate.HasValue)
                    q = q.Where(a => a.CreatedDate >= filter.FromDate.Value);
                if (filter.ToDate.HasValue)
                    q = q.Where(a => a.CreatedDate <= filter.ToDate.Value);
            }

            var list = await q.OrderByDescending(a => a.IsPinned).ThenByDescending(a => a.CreatedDate).ToListAsync();
            return new APIResponse<IEnumerable<AnnouncementDto>>
            {
                Success = true, StatusCode = HttpStatusCode.OK,
                Data = list.Select(MapAnnouncement)
            };
        }

        public async Task<APIResponse<AnnouncementDto>> GetAnnouncementByIdAsync(int id)
        {
            var a = await _context.Announcements.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (a == null)
                return new APIResponse<AnnouncementDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Announcement not found" };
            return new APIResponse<AnnouncementDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = MapAnnouncement(a) };
        }

        public async Task<APIResponse<AnnouncementDto>> CreateAnnouncementAsync(AnnouncementDto dto, string userName)
        {
            var entity = new Announcement
            {
                Title = dto.Title, Content = dto.Content, Scope = dto.Scope,
                TargetReferenceId = dto.TargetReferenceId, Priority = dto.Priority,
                AttachmentPath = dto.AttachmentPath, ImagePath = dto.ImagePath,
                IsPinned = dto.IsPinned, ExpiryDate = dto.ExpiryDate,
                SchoolRegistrationId = GetCurrentSchoolId(), CreatedBy = userName, CreatedDate = DateTime.UtcNow
            };
            _context.Announcements.Add(entity);
            await _context.SaveChangesAsync();
            dto.Id = entity.Id; dto.CreatedBy = userName; dto.CreatedDate = entity.CreatedDate;
            return new APIResponse<AnnouncementDto> { Success = true, StatusCode = HttpStatusCode.Created, Message = "Announcement published", Data = dto };
        }

        public async Task<APIResponse<AnnouncementDto>> UpdateAnnouncementAsync(int id, AnnouncementDto dto, string userName)
        {
            var entity = await _context.Announcements.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (entity == null)
                return new APIResponse<AnnouncementDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Announcement not found" };

            entity.Title = dto.Title; entity.Content = dto.Content; entity.Scope = dto.Scope;
            entity.TargetReferenceId = dto.TargetReferenceId; entity.Priority = dto.Priority;
            entity.AttachmentPath = dto.AttachmentPath; entity.ImagePath = dto.ImagePath;
            entity.IsPinned = dto.IsPinned; entity.ExpiryDate = dto.ExpiryDate;
            entity.UpdatedBy = userName; entity.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return new APIResponse<AnnouncementDto> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Announcement updated", Data = MapAnnouncement(entity) };
        }

        public async Task<APIResponse<bool>> DeleteAnnouncementAsync(int id)
        {
            var entity = await _context.Announcements.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Announcement not found" };
            entity.IsDeleted = true;
            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Announcement deleted", Data = true };
        }

        private static AnnouncementDto MapAnnouncement(Announcement a) => new()
        {
            Id = a.Id, Title = a.Title, Content = a.Content, Scope = a.Scope,
            TargetReferenceId = a.TargetReferenceId, Priority = a.Priority,
            AttachmentPath = a.AttachmentPath, ImagePath = a.ImagePath,
            IsPinned = a.IsPinned, ExpiryDate = a.ExpiryDate,
            CreatedBy = a.CreatedBy, CreatedDate = a.CreatedDate,
            UpdatedBy = a.UpdatedBy, UpdatedDate = a.UpdatedDate
        };


        // ════════════════════════════════════════════════════════════════════════
        // MEETING MANAGEMENT
        // ════════════════════════════════════════════════════════════════════════

        public async Task<APIResponse<IEnumerable<MeetingDto>>> GetMeetingsAsync(MeetingFilterDto? filter = null)
        {
            var q = _context.CommunicationMeetings.Where(m => !m.IsDeleted).AsQueryable();

            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.Platform))
                    q = q.Where(m => m.Platform == filter.Platform);
                if (!string.IsNullOrWhiteSpace(filter.Status))
                    q = q.Where(m => m.Status == filter.Status);
                if (!string.IsNullOrWhiteSpace(filter.TargetAudience) && filter.TargetAudience != "All")
                    q = q.Where(m => m.TargetAudience == filter.TargetAudience);
                if (!string.IsNullOrWhiteSpace(filter.Keyword))
                    q = q.Where(m => m.Title.Contains(filter.Keyword) || m.Description!.Contains(filter.Keyword));
                if (filter.FromDate.HasValue)
                    q = q.Where(m => m.StartTime >= filter.FromDate.Value);
                if (filter.ToDate.HasValue)
                    q = q.Where(m => m.StartTime <= filter.ToDate.Value);
            }

            var list = await q.OrderBy(m => m.StartTime).ToListAsync();
            return new APIResponse<IEnumerable<MeetingDto>>
            {
                Success = true, StatusCode = HttpStatusCode.OK,
                Data = list.Select(MapMeeting)
            };
        }

        public async Task<APIResponse<MeetingDto>> GetMeetingByIdAsync(int id)
        {
            var m = await _context.CommunicationMeetings.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (m == null)
                return new APIResponse<MeetingDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Meeting not found" };
            return new APIResponse<MeetingDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = MapMeeting(m) };
        }

        public async Task<APIResponse<MeetingDto>> CreateMeetingAsync(MeetingDto dto, string userName)
        {
            var entity = new CommunicationMeeting
            {
                Title = dto.Title, Description = dto.Description, StartTime = dto.StartTime, EndTime = dto.EndTime,
                Platform = dto.Platform, MeetingLink = dto.MeetingLink, MeetingId = dto.MeetingId, MeetingPassword = dto.MeetingPassword,
                Agenda = dto.Agenda, MinutesOfMeeting = dto.MinutesOfMeeting, RecordingLink = dto.RecordingLink, Status = dto.Status,
                TargetAudience = dto.TargetAudience, SchoolRegistrationId = GetCurrentSchoolId(), CreatedBy = userName, CreatedDate = DateTime.UtcNow
            };
            _context.CommunicationMeetings.Add(entity);
            await _context.SaveChangesAsync();
            dto.Id = entity.Id; dto.CreatedBy = userName; dto.CreatedDate = entity.CreatedDate;
            return new APIResponse<MeetingDto> { Success = true, StatusCode = HttpStatusCode.Created, Message = "Meeting scheduled", Data = dto };
        }

        public async Task<APIResponse<MeetingDto>> UpdateMeetingAsync(int id, MeetingDto dto, string userName)
        {
            var entity = await _context.CommunicationMeetings.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (entity == null)
                return new APIResponse<MeetingDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Meeting not found" };

            entity.Title = dto.Title; entity.Description = dto.Description;
            entity.StartTime = dto.StartTime; entity.EndTime = dto.EndTime;
            entity.Platform = dto.Platform; entity.MeetingLink = dto.MeetingLink;
            entity.MeetingId = dto.MeetingId; entity.MeetingPassword = dto.MeetingPassword;
            entity.Agenda = dto.Agenda; entity.MinutesOfMeeting = dto.MinutesOfMeeting;
            entity.RecordingLink = dto.RecordingLink; entity.Status = dto.Status;
            entity.TargetAudience = dto.TargetAudience;
            entity.UpdatedBy = userName; entity.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return new APIResponse<MeetingDto> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Meeting updated", Data = MapMeeting(entity) };
        }

        public async Task<APIResponse<bool>> DeleteMeetingAsync(int id)
        {
            var entity = await _context.CommunicationMeetings.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Meeting not found" };
            entity.IsDeleted = true;
            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Meeting deleted", Data = true };
        }

        public async Task<APIResponse<bool>> SaveMinutesOfMeetingAsync(int id, string minutes, string recordingLink, string userName)
        {
            var entity = await _context.CommunicationMeetings.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Meeting not found" };

            entity.MinutesOfMeeting = minutes;
            entity.RecordingLink = recordingLink;
            entity.Status = "Completed";
            entity.UpdatedBy = userName; entity.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Minutes saved and meeting marked completed", Data = true };
        }

        private static MeetingDto MapMeeting(CommunicationMeeting m) => new()
        {
            Id = m.Id, Title = m.Title, Description = m.Description, StartTime = m.StartTime, EndTime = m.EndTime,
            Platform = m.Platform, MeetingLink = m.MeetingLink, MeetingId = m.MeetingId, MeetingPassword = m.MeetingPassword,
            Agenda = m.Agenda, MinutesOfMeeting = m.MinutesOfMeeting, RecordingLink = m.RecordingLink, Status = m.Status,
            TargetAudience = m.TargetAudience, CreatedBy = m.CreatedBy, CreatedDate = m.CreatedDate,
            UpdatedBy = m.UpdatedBy, UpdatedDate = m.UpdatedDate
        };


        // ════════════════════════════════════════════════════════════════════════
        // SUPPORT DESK (TICKET MANAGEMENT)
        // ════════════════════════════════════════════════════════════════════════

        public async Task<APIResponse<IEnumerable<SupportTicketDto>>> GetTicketsAsync(TicketFilterDto? filter = null)
        {
            var q = _context.SupportTickets.Where(t => !t.IsDeleted).AsQueryable();

            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.Category))
                    q = q.Where(t => t.Category == filter.Category);
                if (!string.IsNullOrWhiteSpace(filter.Status))
                    q = q.Where(t => t.Status == filter.Status);
                if (!string.IsNullOrWhiteSpace(filter.Priority))
                    q = q.Where(t => t.Priority == filter.Priority);
                if (!string.IsNullOrWhiteSpace(filter.RaisedByUserId))
                    q = q.Where(t => t.RaisedByUserId == filter.RaisedByUserId);
                if (!string.IsNullOrWhiteSpace(filter.AssignedStaffId))
                    q = q.Where(t => t.AssignedStaffId == filter.AssignedStaffId);
                if (!string.IsNullOrWhiteSpace(filter.Keyword))
                    q = q.Where(t => t.Subject.Contains(filter.Keyword) || t.Description.Contains(filter.Keyword) || t.TicketNumber.Contains(filter.Keyword));
            }

            var list = await q.OrderByDescending(t => t.CreatedDate).ToListAsync();
            var userIds = list.Select(t => t.RaisedByUserId).Concat(list.Where(t => t.AssignedStaffId != null).Select(t => t.AssignedStaffId!)).Distinct().ToList();
            var users = await _context.Users.IgnoreQueryFilters().Where(u => userIds.Contains(u.Id)).ToDictionaryAsync(u => u.Id, u => $"{u.FirstName} {u.LastName}");

            return new APIResponse<IEnumerable<SupportTicketDto>>
            {
                Success = true, StatusCode = HttpStatusCode.OK,
                Data = list.Select(t => new SupportTicketDto
                {
                    Id = t.Id, TicketNumber = t.TicketNumber, Subject = t.Subject, Description = t.Description,
                    Category = t.Category, Status = t.Status, Priority = t.Priority,
                    RaisedByUserId = t.RaisedByUserId, RaisedByUserName = users.GetValueOrDefault(t.RaisedByUserId, "Parent"),
                    AssignedStaffId = t.AssignedStaffId, AssignedStaffName = t.AssignedStaffId != null ? users.GetValueOrDefault(t.AssignedStaffId, "Staff") : "Unassigned",
                    SLAExpiryDate = t.SLAExpiryDate, ResolutionNotes = t.ResolutionNotes, FeedbackRating = t.FeedbackRating,
                    FeedbackComments = t.FeedbackComments, CreatedBy = t.CreatedBy, CreatedDate = t.CreatedDate,
                    UpdatedBy = t.UpdatedBy, UpdatedDate = t.UpdatedDate
                })
            };
        }

        public async Task<APIResponse<SupportTicketDto>> GetTicketByIdAsync(int id)
        {
            var t = await _context.SupportTickets.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (t == null)
                return new APIResponse<SupportTicketDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Ticket not found" };

            var responses = await _context.TicketResponses
                .Where(r => r.TicketId == id && !r.IsDeleted)
                .OrderBy(r => r.CreatedDate)
                .ToListAsync();

            var userIds = new List<string> { t.RaisedByUserId };
            if (t.AssignedStaffId != null) userIds.Add(t.AssignedStaffId);
            userIds.AddRange(responses.Select(r => r.SenderUserId));
            var users = await _context.Users.IgnoreQueryFilters().Where(u => userIds.Distinct().Contains(u.Id)).ToDictionaryAsync(u => u.Id, u => $"{u.FirstName} {u.LastName}");

            var dto = new SupportTicketDto
            {
                Id = t.Id, TicketNumber = t.TicketNumber, Subject = t.Subject, Description = t.Description,
                Category = t.Category, Status = t.Status, Priority = t.Priority,
                RaisedByUserId = t.RaisedByUserId, RaisedByUserName = users.GetValueOrDefault(t.RaisedByUserId, "User"),
                AssignedStaffId = t.AssignedStaffId, AssignedStaffName = t.AssignedStaffId != null ? users.GetValueOrDefault(t.AssignedStaffId, "Staff") : "Unassigned",
                SLAExpiryDate = t.SLAExpiryDate, ResolutionNotes = t.ResolutionNotes, FeedbackRating = t.FeedbackRating,
                FeedbackComments = t.FeedbackComments, CreatedBy = t.CreatedBy, CreatedDate = t.CreatedDate,
                UpdatedBy = t.UpdatedBy, UpdatedDate = t.UpdatedDate,
                Responses = responses.Select(r => new TicketResponseDto
                {
                    Id = r.Id, TicketId = r.TicketId, SenderUserId = r.SenderUserId,
                    SenderUserName = users.GetValueOrDefault(r.SenderUserId, "User"),
                    Content = r.Content, IsInternalNote = r.IsInternalNote, AttachmentPath = r.AttachmentPath,
                    CreatedDate = r.CreatedDate ?? DateTime.UtcNow
                }).ToList()
            };

            return new APIResponse<SupportTicketDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = dto };
        }

        public async Task<APIResponse<SupportTicketDto>> CreateTicketAsync(SupportTicketDto dto, string userName)
        {
            var count = await _context.SupportTickets.IgnoreQueryFilters().CountAsync();
            var ticketNo = $"TKT-{DateTime.UtcNow.Year}-{(count + 1):D4}";

            var entity = new SupportTicket
            {
                TicketNumber = ticketNo, Subject = dto.Subject, Description = dto.Description,
                Category = dto.Category, Status = "Open", Priority = dto.Priority,
                RaisedByUserId = dto.RaisedByUserId, AssignedStaffId = dto.AssignedStaffId,
                SLAExpiryDate = DateTime.UtcNow.AddDays(dto.Priority == "Urgent" ? 1 : dto.Priority == "High" ? 2 : 5),
                SchoolRegistrationId = GetCurrentSchoolId(), CreatedBy = userName, CreatedDate = DateTime.UtcNow
            };

            _context.SupportTickets.Add(entity);
            await _context.SaveChangesAsync();

            // Trigger system notification to admin/staff
            await SendSystemNotificationAsync("2", "New Support Ticket raised", $"Ticket {ticketNo} - {dto.Subject} has been registered.", "Support", dto.Priority, "/communication");

            dto.Id = entity.Id; dto.TicketNumber = ticketNo; dto.Status = "Open"; dto.CreatedDate = entity.CreatedDate;
            return new APIResponse<SupportTicketDto> { Success = true, StatusCode = HttpStatusCode.Created, Message = "Ticket raised successfully", Data = dto };
        }

        public async Task<APIResponse<SupportTicketDto>> ReplyToTicketAsync(int ticketId, TicketResponseDto dto, string userName)
        {
            var ticket = await _context.SupportTickets.FirstOrDefaultAsync(t => t.Id == ticketId && !t.IsDeleted);
            if (ticket == null)
                return new APIResponse<SupportTicketDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Ticket not found" };

            var entity = new TicketResponse
            {
                TicketId = ticketId, SenderUserId = dto.SenderUserId, Content = dto.Content,
                IsInternalNote = dto.IsInternalNote, AttachmentPath = dto.AttachmentPath,
                SchoolRegistrationId = GetCurrentSchoolId(), CreatedBy = userName, CreatedDate = DateTime.UtcNow
            };

            _context.TicketResponses.Add(entity);

            if (!dto.IsInternalNote)
            {
                ticket.Status = ticket.RaisedByUserId == dto.SenderUserId ? "Open" : "InProgress";
                ticket.UpdatedBy = userName; ticket.UpdatedDate = DateTime.UtcNow;

                // Send notification to recipient
                var recipientId = ticket.RaisedByUserId == dto.SenderUserId ? ticket.AssignedStaffId ?? "2" : ticket.RaisedByUserId;
                await SendSystemNotificationAsync(recipientId, "Support Ticket Update", $"New response posted for Ticket {ticket.TicketNumber}.", "Support", ticket.Priority, "/communication");
            }

            await _context.SaveChangesAsync();
            return await GetTicketByIdAsync(ticketId);
        }

        public async Task<APIResponse<SupportTicketDto>> ResolveTicketAsync(int ticketId, string resolutionNotes, string userName)
        {
            var ticket = await _context.SupportTickets.FirstOrDefaultAsync(t => t.Id == ticketId && !t.IsDeleted);
            if (ticket == null)
                return new APIResponse<SupportTicketDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Ticket not found" };

            ticket.Status = "Resolved";
            ticket.ResolutionNotes = resolutionNotes;
            ticket.UpdatedBy = userName; ticket.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            await SendSystemNotificationAsync(ticket.RaisedByUserId, "Support Ticket Resolved", $"Your Ticket {ticket.TicketNumber} has been resolved.", "Support", ticket.Priority, "/communication");

            return await GetTicketByIdAsync(ticketId);
        }


        // ════════════════════════════════════════════════════════════════════════
        // QUICK POLLS
        // ════════════════════════════════════════════════════════════════════════

        public async Task<APIResponse<IEnumerable<QuickPollDto>>> GetPollsAsync(string userId, bool? isActive = null)
        {
            var q = _context.QuickPolls.Where(p => !p.IsDeleted).AsQueryable();
            if (isActive.HasValue)
                q = q.Where(p => p.IsActive == isActive.Value);

            var polls = await q.OrderByDescending(p => p.CreatedDate).ToListAsync();
            var pollIds = polls.Select(p => p.Id).ToList();

            var votes = await _context.PollVotes
                .Where(v => pollIds.Contains(v.PollId) && !v.IsDeleted)
                .ToListAsync();

            var userVotes = votes.Where(v => v.UserId == userId).ToDictionary(v => v.PollId, v => v.SelectedOption);

            var result = polls.Select(p => new QuickPollDto
            {
                Id = p.Id, Question = p.Question, OptionsJson = p.OptionsJson,
                StartDate = p.StartDate, EndDate = p.EndDate, IsActive = p.IsActive,
                TargetAudience = p.TargetAudience, CreatedBy = p.CreatedBy, CreatedDate = p.CreatedDate,
                UserHasVoted = userVotes.ContainsKey(p.Id),
                UserVotedOption = userVotes.GetValueOrDefault(p.Id),
                Votes = votes.Where(v => v.PollId == p.Id).Select(v => new PollVoteDto
                {
                    Id = v.Id, PollId = v.PollId, UserId = v.UserId, SelectedOption = v.SelectedOption, VotedAt = v.VotedAt
                }).ToList()
            });

            return new APIResponse<IEnumerable<QuickPollDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = result };
        }

        public async Task<APIResponse<QuickPollDto>> CreatePollAsync(QuickPollDto dto, string userName)
        {
            var entity = new QuickPoll
            {
                Question = dto.Question, OptionsJson = dto.OptionsJson,
                StartDate = dto.StartDate == default ? DateTime.UtcNow : dto.StartDate,
                EndDate = dto.EndDate == default ? DateTime.UtcNow.AddDays(7) : dto.EndDate,
                IsActive = dto.IsActive, TargetAudience = dto.TargetAudience,
                SchoolRegistrationId = GetCurrentSchoolId(), CreatedBy = userName, CreatedDate = DateTime.UtcNow
            };
            _context.QuickPolls.Add(entity);
            await _context.SaveChangesAsync();
            dto.Id = entity.Id; dto.CreatedBy = userName; dto.CreatedDate = entity.CreatedDate;
            return new APIResponse<QuickPollDto> { Success = true, StatusCode = HttpStatusCode.Created, Message = "Poll published", Data = dto };
        }

        public async Task<APIResponse<bool>> VoteInPollAsync(int pollId, string userId, string selectedOption)
        {
            var poll = await _context.QuickPolls.FirstOrDefaultAsync(p => p.Id == pollId && p.IsActive && !p.IsDeleted);
            if (poll == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Active poll not found" };

            var existing = await _context.PollVotes.FirstOrDefaultAsync(v => v.PollId == pollId && v.UserId == userId && !v.IsDeleted);
            if (existing != null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.BadRequest, Message = "User has already voted" };

            var vote = new PollVote
            {
                PollId = pollId, UserId = userId, SelectedOption = selectedOption, VotedAt = DateTime.UtcNow,
                SchoolRegistrationId = GetCurrentSchoolId()
            };
            _context.PollVotes.Add(vote);
            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Vote recorded", Data = true };
        }

        public async Task<APIResponse<IEnumerable<PollResultDto>>> GetPollResultsAsync(int pollId)
        {
            var poll = await _context.QuickPolls.FirstOrDefaultAsync(p => p.Id == pollId && !p.IsDeleted);
            if (poll == null)
                return new APIResponse<IEnumerable<PollResultDto>> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Poll not found" };

            var votes = await _context.PollVotes.Where(v => v.PollId == pollId && !v.IsDeleted).ToListAsync();
            var totalVotes = votes.Count;

            // Deserialize options
            var options = System.Text.Json.JsonSerializer.Deserialize<List<string>>(poll.OptionsJson) ?? new List<string>();

            var results = options.Select(opt =>
            {
                var count = votes.Count(v => v.SelectedOption == opt);
                return new PollResultDto
                {
                    Option = opt, VoteCount = count,
                    Percentage = totalVotes > 0 ? Math.Round((double)count / totalVotes * 100, 1) : 0
                };
            });

            return new APIResponse<IEnumerable<PollResultDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = results };
        }


        // ════════════════════════════════════════════════════════════════════════
        // DOCUMENT SHARING
        // ════════════════════════════════════════════════════════════════════════

        public async Task<APIResponse<IEnumerable<SharedDocumentDto>>> GetSharedDocumentsAsync(DocumentFilterDto? filter = null)
        {
            var q = _context.SharedDocuments.Where(d => !d.IsDeleted).AsQueryable();

            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.TargetAudience) && filter.TargetAudience != "All")
                    q = q.Where(d => d.TargetAudience == filter.TargetAudience);
                if (!string.IsNullOrWhiteSpace(filter.FileType))
                    q = q.Where(d => d.FileType == filter.FileType);
                if (!string.IsNullOrWhiteSpace(filter.Keyword))
                    q = q.Where(d => d.FileName.Contains(filter.Keyword) || d.Description!.Contains(filter.Keyword));
            }

            var list = await q.OrderByDescending(d => d.CreatedDate).ToListAsync();
            return new APIResponse<IEnumerable<SharedDocumentDto>>
            {
                Success = true, StatusCode = HttpStatusCode.OK,
                Data = list.Select(d => new SharedDocumentDto
                {
                    Id = d.Id, FileName = d.FileName, Description = d.Description, FilePath = d.FilePath,
                    FileSize = d.FileSize, FileType = d.FileType, TargetAudience = d.TargetAudience,
                    ExpiryDate = d.ExpiryDate, DownloadCount = d.DownloadCount, IsPublicLink = d.IsPublicLink,
                    CreatedBy = d.CreatedBy, CreatedDate = d.CreatedDate, UpdatedBy = d.UpdatedBy, UpdatedDate = d.UpdatedDate
                })
            };
        }

        public async Task<APIResponse<SharedDocumentDto>> ShareDocumentAsync(SharedDocumentDto dto, string userName)
        {
            var entity = new SharedDocument
            {
                FileName = dto.FileName, Description = dto.Description, FilePath = dto.FilePath,
                FileSize = dto.FileSize, FileType = dto.FileType, TargetAudience = dto.TargetAudience,
                ExpiryDate = dto.ExpiryDate, IsPublicLink = dto.IsPublicLink, DownloadCount = 0,
                SchoolRegistrationId = GetCurrentSchoolId(), CreatedBy = userName, CreatedDate = DateTime.UtcNow
            };
            _context.SharedDocuments.Add(entity);
            await _context.SaveChangesAsync();
            dto.Id = entity.Id; dto.CreatedBy = userName; dto.CreatedDate = entity.CreatedDate;
            return new APIResponse<SharedDocumentDto> { Success = true, StatusCode = HttpStatusCode.Created, Message = "Document shared", Data = dto };
        }

        public async Task<APIResponse<bool>> DeleteSharedDocumentAsync(int id)
        {
            var entity = await _context.SharedDocuments.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Document not found" };
            entity.IsDeleted = true;
            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Document deleted", Data = true };
        }

        public async Task<APIResponse<bool>> TrackDocumentDownloadAsync(int id)
        {
            var entity = await _context.SharedDocuments.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Document not found" };
            entity.DownloadCount++;
            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Download tracked", Data = true };
        }


        // ════════════════════════════════════════════════════════════════════════
        // TEMPLATE MANAGEMENT
        // ════════════════════════════════════════════════════════════════════════

        public async Task<APIResponse<IEnumerable<TemplateDto>>> GetTemplatesAsync(TemplateFilterDto? filter = null)
        {
            var q = _context.CommunicationTemplates.Where(t => !t.IsDeleted).AsQueryable();

            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.Type))
                    q = q.Where(t => t.Type == filter.Type);
                if (!string.IsNullOrWhiteSpace(filter.Keyword))
                    q = q.Where(t => t.Name.Contains(filter.Keyword) || t.BodyTemplate.Contains(filter.Keyword));
            }

            var list = await q.OrderByDescending(t => t.CreatedDate).ToListAsync();
            return new APIResponse<IEnumerable<TemplateDto>>
            {
                Success = true, StatusCode = HttpStatusCode.OK,
                Data = list.Select(t => new TemplateDto
                {
                    Id = t.Id, Name = t.Name, Type = t.Type, SubjectTemplate = t.SubjectTemplate,
                    BodyTemplate = t.BodyTemplate, IsActive = t.IsActive, CreatedBy = t.CreatedBy, CreatedDate = t.CreatedDate ?? DateTime.UtcNow
                })
            };
        }

        public async Task<APIResponse<TemplateDto>> CreateTemplateAsync(TemplateDto dto, string userName)
        {
            var entity = new CommunicationTemplate
            {
                Name = dto.Name, Type = dto.Type, SubjectTemplate = dto.SubjectTemplate,
                BodyTemplate = dto.BodyTemplate, IsActive = dto.IsActive,
                SchoolRegistrationId = GetCurrentSchoolId(), CreatedBy = userName, CreatedDate = DateTime.UtcNow
            };
            _context.CommunicationTemplates.Add(entity);
            await _context.SaveChangesAsync();
            dto.Id = entity.Id; dto.CreatedBy = userName; dto.CreatedDate = entity.CreatedDate ?? DateTime.UtcNow;
            return new APIResponse<TemplateDto> { Success = true, StatusCode = HttpStatusCode.Created, Message = "Template saved", Data = dto };
        }

        public async Task<APIResponse<TemplateDto>> UpdateTemplateAsync(int id, TemplateDto dto, string userName)
        {
            var entity = await _context.CommunicationTemplates.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (entity == null)
                return new APIResponse<TemplateDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Template not found" };

            entity.Name = dto.Name; entity.Type = dto.Type;
            entity.SubjectTemplate = dto.SubjectTemplate; entity.BodyTemplate = dto.BodyTemplate;
            entity.IsActive = dto.IsActive;
            entity.UpdatedBy = userName; entity.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return new APIResponse<TemplateDto>
            {
                Success = true, StatusCode = HttpStatusCode.OK, Message = "Template updated",
                Data = new TemplateDto { Id = entity.Id, Name = entity.Name, Type = entity.Type, SubjectTemplate = entity.SubjectTemplate, BodyTemplate = entity.BodyTemplate, IsActive = entity.IsActive, CreatedBy = entity.CreatedBy, CreatedDate = entity.CreatedDate ?? DateTime.UtcNow }
            };
        }

        public async Task<APIResponse<bool>> DeleteTemplateAsync(int id)
        {
            var entity = await _context.CommunicationTemplates.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Template not found" };
            entity.IsDeleted = true;
            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Template deleted", Data = true };
        }


        // ════════════════════════════════════════════════════════════════════════
        // GROUP CHATS / CHANNEL MANAGEMENT
        // ════════════════════════════════════════════════════════════════════════

        public async Task<APIResponse<IEnumerable<GroupChatRoomDto>>> GetChatRoomsAsync(string userId)
        {
            // Fetch rooms where the user is a member
            var roomIds = await _context.GroupChatMembers
                .Where(m => m.UserId == userId && !m.IsDeleted)
                .Select(m => m.RoomId)
                .ToListAsync();

            var rooms = await _context.GroupChatRooms
                .Where(r => roomIds.Contains(r.Id) && !r.IsDeleted)
                .ToListAsync();

            var results = new List<GroupChatRoomDto>();
            foreach (var r in rooms)
            {
                var memberCount = await _context.GroupChatMembers.CountAsync(m => m.RoomId == r.Id && !m.IsDeleted);
                var lastMsg = await _context.GroupChatMessages
                    .Where(m => m.RoomId == r.Id && !m.IsDeleted)
                    .OrderByDescending(m => m.SentTime)
                    .FirstOrDefaultAsync();

                results.Add(new GroupChatRoomDto
                {
                    Id = r.Id, Name = r.Name, Type = r.Type, TargetReferenceId = r.TargetReferenceId,
                    MemberCount = memberCount, LastMessageContent = lastMsg?.MessageContent ?? "No messages yet",
                    LastMessageTime = lastMsg?.SentTime
                });
            }

            return new APIResponse<IEnumerable<GroupChatRoomDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = results };
        }

        public async Task<APIResponse<GroupChatRoomDto>> CreateChatRoomAsync(GroupChatRoomDto dto, string userName)
        {
            var room = new GroupChatRoom
            {
                Name = dto.Name, Type = dto.Type, TargetReferenceId = dto.TargetReferenceId,
                SchoolRegistrationId = GetCurrentSchoolId(), CreatedBy = userName, CreatedDate = DateTime.UtcNow
            };
            _context.GroupChatRooms.Add(room);
            await _context.SaveChangesAsync();

            // Auto add members if Department or Class
            if (dto.Type == "Class" && int.TryParse(dto.TargetReferenceId, out var classId))
            {
                var studentUserIds = await _context.Students.Where(s => s.ClassId == classId && !s.IsDeleted).Select(s => s.ApplicationUserId).ToListAsync();
                foreach (var uid in studentUserIds)
                {
                    _context.GroupChatMembers.Add(new GroupChatMember { RoomId = room.Id, UserId = uid, SchoolRegistrationId = room.SchoolRegistrationId, JoinedAt = DateTime.UtcNow });
                }
            }

            // Always add the creator as member/admin
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user != null)
            {
                _context.GroupChatMembers.Add(new GroupChatMember { RoomId = room.Id, UserId = user.Id, IsAdmin = true, SchoolRegistrationId = room.SchoolRegistrationId, JoinedAt = DateTime.UtcNow });
            }

            await _context.SaveChangesAsync();
            dto.Id = room.Id;
            return new APIResponse<GroupChatRoomDto> { Success = true, StatusCode = HttpStatusCode.Created, Message = "Group chat room created", Data = dto };
        }

        public async Task<APIResponse<bool>> AddRoomMemberAsync(int roomId, string userId)
        {
            var exists = await _context.GroupChatMembers.AnyAsync(m => m.RoomId == roomId && m.UserId == userId && !m.IsDeleted);
            if (exists)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.BadRequest, Message = "Already a member" };

            var member = new GroupChatMember
            {
                RoomId = roomId, UserId = userId, JoinedAt = DateTime.UtcNow,
                SchoolRegistrationId = GetCurrentSchoolId()
            };
            _context.GroupChatMembers.Add(member);
            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Member added", Data = true };
        }

        public async Task<APIResponse<IEnumerable<GroupChatMessageDto>>> GetGroupMessagesAsync(int roomId)
        {
            var list = await _context.GroupChatMessages
                .Where(m => m.RoomId == roomId && !m.IsDeleted)
                .OrderBy(m => m.SentTime)
                .ToListAsync();

            var userIds = list.Select(m => m.SenderUserId).Distinct().ToList();
            var users = await _context.Users.IgnoreQueryFilters()
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => new { Name = $"{u.FirstName} {u.LastName}", Role = u.UserName ?? "User" });

            var result = list.Select(m => new GroupChatMessageDto
            {
                Id = m.Id, RoomId = m.RoomId, SenderUserId = m.SenderUserId,
                SenderUserName = users.ContainsKey(m.SenderUserId) ? users[m.SenderUserId].Name : "User",
                SenderRoleName = users.ContainsKey(m.SenderUserId) ? users[m.SenderUserId].Role : "Guest",
                MessageContent = m.MessageContent, AttachmentPath = m.AttachmentPath, SentTime = m.SentTime
            });

            return new APIResponse<IEnumerable<GroupChatMessageDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = result };
        }

        public async Task<APIResponse<GroupChatMessageDto>> SendGroupMessageAsync(GroupChatMessageDto dto, string userName)
        {
            var entity = new GroupChatMessage
            {
                RoomId = dto.RoomId, SenderUserId = dto.SenderUserId, MessageContent = dto.MessageContent,
                AttachmentPath = dto.AttachmentPath, SentTime = DateTime.UtcNow,
                SchoolRegistrationId = GetCurrentSchoolId(), CreatedBy = userName, CreatedDate = DateTime.UtcNow
            };
            _context.GroupChatMessages.Add(entity);
            await _context.SaveChangesAsync();
            dto.Id = entity.Id; dto.SentTime = entity.SentTime;
            return new APIResponse<GroupChatMessageDto> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Group message posted", Data = dto };
        }


        // ════════════════════════════════════════════════════════════════════════
        // CENTRAL NOTIFICATION INBOX
        // ════════════════════════════════════════════════════════════════════════

        public async Task<APIResponse<IEnumerable<CentralNotificationDto>>> GetUserNotificationsAsync(string userId, NotificationFilterDto? filter = null)
        {
            var q = _context.CentralNotifications.Where(n => n.RecipientUserId == userId && !n.IsDeleted).AsQueryable();

            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.Category))
                    q = q.Where(n => n.Category == filter.Category);
                if (!string.IsNullOrWhiteSpace(filter.Priority))
                    q = q.Where(n => n.Priority == filter.Priority);
                if (filter.IsRead.HasValue)
                    q = q.Where(n => n.IsRead == filter.IsRead.Value);
                if (filter.IsStarred.HasValue)
                    q = q.Where(n => n.IsStarred == filter.IsStarred.Value);
                if (filter.IsArchived.HasValue)
                    q = q.Where(n => n.IsArchived == filter.IsArchived.Value);
            }

            var list = await q.OrderByDescending(n => n.SentDate).ToListAsync();
            return new APIResponse<IEnumerable<CentralNotificationDto>>
            {
                Success = true, StatusCode = HttpStatusCode.OK,
                Data = list.Select(n => new CentralNotificationDto
                {
                    Id = n.Id, RecipientUserId = n.RecipientUserId, Title = n.Title, Body = n.Body,
                    Category = n.Category, Priority = n.Priority, ActionUrl = n.ActionUrl,
                    IsRead = n.IsRead, IsStarred = n.IsStarred, IsArchived = n.IsArchived, SentDate = n.SentDate
                })
            };
        }

        public async Task<APIResponse<bool>> MarkNotificationReadAsync(int id, string userName)
        {
            var entity = await _context.CentralNotifications.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Notification not found" };

            entity.IsRead = true;
            entity.UpdatedBy = userName; entity.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Marked read", Data = true };
        }

        public async Task<APIResponse<bool>> ToggleNotificationStarAsync(int id, string userName)
        {
            var entity = await _context.CentralNotifications.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Notification not found" };

            entity.IsStarred = !entity.IsStarred;
            entity.UpdatedBy = userName; entity.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Starred toggled", Data = true };
        }

        public async Task<APIResponse<bool>> ToggleNotificationArchiveAsync(int id, string userName)
        {
            var entity = await _context.CentralNotifications.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Notification not found" };

            entity.IsArchived = !entity.IsArchived;
            entity.UpdatedBy = userName; entity.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Archive toggled", Data = true };
        }

        public async Task<APIResponse<bool>> SendSystemNotificationAsync(string recipientUserId, string title, string body, string category, string priority, string? actionUrl)
        {
            var notification = new CentralNotification
            {
                RecipientUserId = recipientUserId, Title = title, Body = body,
                Category = category, Priority = priority, ActionUrl = actionUrl,
                IsRead = false, IsStarred = false, IsArchived = false, SentDate = DateTime.UtcNow,
                SchoolRegistrationId = GetCurrentSchoolId(), CreatedBy = "system", CreatedDate = DateTime.UtcNow
            };
            _context.CentralNotifications.Add(notification);
            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Message = "Alert logged", Data = true };
        }


        // ════════════════════════════════════════════════════════════════════════
        // ANALYTICS & DASHBOARD METHODS
        // ════════════════════════════════════════════════════════════════════════

        public async Task<APIResponse<CommunicationDashboardStatsDto>> GetCommunicationDashboardStatsAsync()
        {
            var schoolId = GetCurrentSchoolId();
            var stats = new CommunicationDashboardStatsDto
            {
                MessagesSentToday = await _context.ParentTeacherChats.CountAsync(c => c.SentTime.Date == DateTime.UtcNow.Date) +
                                    await _context.GroupChatMessages.CountAsync(c => c.SentTime.Date == DateTime.UtcNow.Date),
                UnreadMessages = await _context.ParentTeacherChats.CountAsync(c => !c.IsRead) +
                                 await _context.CentralNotifications.CountAsync(n => !n.IsRead),
                EmailsSent = await _context.EmailLogs.CountAsync(),
                SmsSent = await _context.SmsLogs.CountAsync(),
                WhatsAppSent = await _context.WhatsAppLogs.CountAsync(),
                PushNotificationsSent = await _context.PushNotifications.CountAsync(),
                AnnouncementsCount = await _context.Announcements.CountAsync(a => !a.IsDeleted),
                CircularsCount = await _context.Circulars.CountAsync(c => !c.IsDeleted),
                NoticesCount = await _context.NoticeBoards.CountAsync(n => !n.IsDeleted),
                EventsCount = await _context.Events.CountAsync(e => e.IsActive),
                MeetingsCount = await _context.CommunicationMeetings.CountAsync(m => !m.IsDeleted),
                OnlineClassesCount = await _context.OnlineClasses.CountAsync(c => c.ScheduledAt.Date == DateTime.UtcNow.Date),
                ActiveSupportTickets = await _context.SupportTickets.CountAsync(t => t.Status != "Closed" && t.Status != "Resolved" && !t.IsDeleted),
                ChatConversationsCount = await _context.GroupChatRooms.CountAsync(r => !r.IsDeleted),
                PendingApprovalsCount = 3, // Mock configuration
                ScheduledMessagesCount = 5,
                FailedDeliveriesCount = await _context.SmsLogs.CountAsync(l => l.SentStatus == "Failed") + await _context.WhatsAppLogs.CountAsync(l => l.Status == "Failed"),
                DeliverySuccessRate = 98.4
            };

            return new APIResponse<CommunicationDashboardStatsDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = stats };
        }

        public async Task<APIResponse<IEnumerable<AnalyticsChartDataDto>>> GetAnalyticsChartDataAsync()
        {
            var charts = new List<AnalyticsChartDataDto>
            {
                new AnalyticsChartDataDto
                {
                    ChartName = "Daily Messages",
                    Labels = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" },
                    Values = new List<int> { 120, 150, 180, 160, 210, 80, 45 }
                },
                new AnalyticsChartDataDto
                {
                    ChartName = "Channel Usage",
                    Labels = new List<string> { "Email", "SMS", "WhatsApp", "Push Notify", "Chat" },
                    Values = new List<int> { 350, 480, 620, 890, 1200 }
                },
                new AnalyticsChartDataDto
                {
                    ChartName = "Read Rate",
                    Labels = new List<string> { "Announcements", "Notice Boards", "Circulars", "Chats", "Push" },
                    Values = new List<int> { 85, 78, 92, 98, 70 } // Percentage values
                }
            };

            return new APIResponse<IEnumerable<AnalyticsChartDataDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = charts };
        }


        // ════════════════════════════════════════════════════════════════════════
        // AI MESSAGE HELPERS (SIMULATION)
        // ════════════════════════════════════════════════════════════════════════

        public async Task<APIResponse<string>> GenerateAISmartMessageAsync(string prompt, string channelType)
        {
            await Task.Delay(500); // Simulate network lag
            string smartResponse;

            if (channelType.Equals("Email", StringComparison.OrdinalIgnoreCase))
            {
                smartResponse = $"Dear Recipient,\n\nBased on your request regarding '{prompt}', we would like to confirm that we are actively reviewing the details. The complete summary will be dispatched shortly.\n\nThank you for your cooperation.\n\nBest Regards,\nSchool Administration";
            }
            else if (channelType.Equals("WhatsApp", StringComparison.OrdinalIgnoreCase))
            {
                smartResponse = $"🔔 *School Update:* Regarding '{prompt}' - we have verified the schedules. Please log in to your parent dashboard for final guidelines. Thank you!";
            }
            else
            {
                smartResponse = $"ERP ALERT: Regarding '{prompt}' - guidelines updated on school portal. Check dashboard.";
            }

            return new APIResponse<string> { Success = true, StatusCode = HttpStatusCode.OK, Data = smartResponse };
        }

        public async Task<APIResponse<string>> AnalyzeSentimentAsync(string text)
        {
            await Task.Delay(200);
            var lowercase = text.ToLower();
            string sentiment = "Neutral";

            if (lowercase.Contains("angry") || lowercase.Contains("terrible") || lowercase.Contains("bad") || lowercase.Contains("late") || lowercase.Contains("worst") || lowercase.Contains("delay"))
            {
                sentiment = "Negative";
            }
            else if (lowercase.Contains("excellent") || lowercase.Contains("great") || lowercase.Contains("good") || lowercase.Contains("happy") || lowercase.Contains("thanks") || lowercase.Contains("success"))
            {
                sentiment = "Positive";
            }

            return new APIResponse<string> { Success = true, StatusCode = HttpStatusCode.OK, Data = sentiment };
        }
    }
}
