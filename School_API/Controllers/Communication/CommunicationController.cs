using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using School.Services.Interfaces;
using School_API.Common.Interface;
using School_DTOs.Communication;

namespace School_API.Controllers.Communication
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommunicationController : BaseController
    {
        private readonly ICommunicationService _svc;

        public CommunicationController(ICommunicationService svc, ICurrentUserService cur) : base(cur)
        {
            _svc = svc;
        }

        // ═══════════════════════════════════════════════════════════════════════
        // NOTICE BOARD
        // ═══════════════════════════════════════════════════════════════════════

        [HttpGet]
        public async Task<IActionResult> GetNotices(
            [FromQuery] string? targetAudience, [FromQuery] bool? isPinned,
            [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var filter = new NoticeBoardFilterDto { TargetAudience = targetAudience, IsPinned = isPinned, FromDate = fromDate, ToDate = toDate };
            var r = await _svc.GetNoticesAsync(filter);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNoticeById(int id)
        {
            var r = await _svc.GetNoticeByIdAsync(id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotice([FromBody] NoticeBoardDto dto)
        {
            var r = await _svc.CreateNoticeAsync(dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotice(int id, [FromBody] NoticeBoardDto dto)
        {
            var r = await _svc.UpdateNoticeAsync(id, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotice(int id)
        {
            var r = await _svc.DeleteNoticeAsync(id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // CIRCULARS
        // ═══════════════════════════════════════════════════════════════════════

        [HttpGet]
        public async Task<IActionResult> GetCirculars(
            [FromQuery] string? targetAudience, [FromQuery] string? keyword,
            [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var filter = new CircularFilterDto { TargetAudience = targetAudience, Keyword = keyword, FromDate = fromDate, ToDate = toDate };
            var r = await _svc.GetCircularsAsync(filter);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCircularById(int id)
        {
            var r = await _svc.GetCircularByIdAsync(id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCircular([FromBody] CircularDto dto)
        {
            var r = await _svc.CreateCircularAsync(dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCircular(int id, [FromBody] CircularDto dto)
        {
            var r = await _svc.UpdateCircularAsync(id, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCircular(int id)
        {
            var r = await _svc.DeleteCircularAsync(id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // SMS GATEWAY
        // ═══════════════════════════════════════════════════════════════════════

        [HttpGet]
        public async Task<IActionResult> GetSmsLogs(
            [FromQuery] string? recipientNo, [FromQuery] string? status,
            [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var filter = new GatewayLogFilterDto { RecipientNo = recipientNo, Status = status, FromDate = fromDate, ToDate = toDate };
            var r = await _svc.GetSmsLogsAsync(filter);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSmsLogById(int id)
        {
            var r = await _svc.GetSmsLogByIdAsync(id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> SendSms([FromBody] SmsLogDto dto)
        {
            var r = await _svc.SendSmsAsync(dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // WHATSAPP GATEWAY
        // ═══════════════════════════════════════════════════════════════════════

        [HttpGet]
        public async Task<IActionResult> GetWhatsAppLogs(
            [FromQuery] string? recipientPhone, [FromQuery] string? status,
            [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var filter = new GatewayLogFilterDto { RecipientNo = recipientPhone, Status = status, FromDate = fromDate, ToDate = toDate };
            var r = await _svc.GetWhatsAppLogsAsync(filter);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWhatsAppLogById(int id)
        {
            var r = await _svc.GetWhatsAppLogByIdAsync(id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> SendWhatsApp([FromBody] WhatsAppLogDto dto)
        {
            var r = await _svc.SendWhatsAppAsync(dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // PUSH NOTIFICATIONS
        // ═══════════════════════════════════════════════════════════════════════

        [HttpGet]
        public async Task<IActionResult> GetPushNotifications(
            [FromQuery] string userId, [FromQuery] string? status,
            [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var filter = new GatewayLogFilterDto { Status = status, FromDate = fromDate, ToDate = toDate };
            var r = await _svc.GetPushNotificationsAsync(userId, filter);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPushNotificationById(int id)
        {
            var r = await _svc.GetPushNotificationByIdAsync(id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> SendPushNotification([FromBody] PushNotificationDto dto)
        {
            var r = await _svc.SendPushNotificationAsync(dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> MarkPushRead(int id)
        {
            var r = await _svc.MarkPushNotificationReadAsync(id, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // PARENT-TEACHER CHAT
        // ═══════════════════════════════════════════════════════════════════════

        [HttpGet]
        public async Task<IActionResult> GetChatHistory([FromQuery] string senderId, [FromQuery] string receiverId)
        {
            var r = await _svc.GetChatHistoryAsync(senderId, receiverId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChatMessageById(int id)
        {
            var r = await _svc.GetChatMessageByIdAsync(id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> SendChatMessage([FromBody] ParentTeacherChatDto dto)
        {
            var r = await _svc.SendChatMessageAsync(dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> MarkChatRead(int id)
        {
            var r = await _svc.MarkChatMessageReadAsync(id, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChatMessage(int id)
        {
            var r = await _svc.DeleteChatMessageAsync(id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // SURVEYS & FEEDBACK
        // ═══════════════════════════════════════════════════════════════════════

        [HttpGet]
        public async Task<IActionResult> GetSurveys(
            [FromQuery] string? targetAudience, [FromQuery] bool? isActive, [FromQuery] string? keyword)
        {
            var filter = new SurveyFilterDto { TargetAudience = targetAudience, IsActive = isActive, Keyword = keyword };
            var r = await _svc.GetSurveysAsync(filter);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSurveyById(int id)
        {
            var r = await _svc.GetSurveyByIdAsync(id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSurvey([FromBody] FeedbackSurveyDto dto)
        {
            var r = await _svc.CreateSurveyAsync(dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSurvey(int id, [FromBody] FeedbackSurveyDto dto)
        {
            var r = await _svc.UpdateSurveyAsync(id, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSurvey(int id)
        {
            var r = await _svc.DeleteSurveyAsync(id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitSurveyResponse([FromBody] SurveyResponseDto dto)
        {
            var r = await _svc.SubmitSurveyResponseAsync(dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{surveyId}")]
        public async Task<IActionResult> GetSurveyResponses(int surveyId)
        {
            var r = await _svc.GetSurveyResponsesAsync(surveyId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // ANNOUNCEMENT MANAGEMENT
        // ═══════════════════════════════════════════════════════════════════════

        [HttpGet]
        public async Task<IActionResult> GetAnnouncements(
            [FromQuery] string? scope, [FromQuery] string? targetReferenceId, [FromQuery] string? priority,
            [FromQuery] string? keyword, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var filter = new AnnouncementFilterDto { Scope = scope, TargetReferenceId = targetReferenceId, Priority = priority, Keyword = keyword, FromDate = fromDate, ToDate = toDate };
            var r = await _svc.GetAnnouncementsAsync(filter);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnnouncementById(int id)
        {
            var r = await _svc.GetAnnouncementByIdAsync(id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnnouncement([FromBody] AnnouncementDto dto)
        {
            var r = await _svc.CreateAnnouncementAsync(dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnnouncement(int id, [FromBody] AnnouncementDto dto)
        {
            var r = await _svc.UpdateAnnouncementAsync(id, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnnouncement(int id)
        {
            var r = await _svc.DeleteAnnouncementAsync(id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // MEETING MANAGEMENT
        // ═══════════════════════════════════════════════════════════════════════

        [HttpGet]
        public async Task<IActionResult> GetMeetings(
            [FromQuery] string? platform, [FromQuery] string? status, [FromQuery] string? targetAudience,
            [FromQuery] string? keyword, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var filter = new MeetingFilterDto { Platform = platform, Status = status, TargetAudience = targetAudience, Keyword = keyword, FromDate = fromDate, ToDate = toDate };
            var r = await _svc.GetMeetingsAsync(filter);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMeetingById(int id)
        {
            var r = await _svc.GetMeetingByIdAsync(id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMeeting([FromBody] MeetingDto dto)
        {
            var r = await _svc.CreateMeetingAsync(dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMeeting(int id, [FromBody] MeetingDto dto)
        {
            var r = await _svc.UpdateMeetingAsync(id, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeeting(int id)
        {
            var r = await _svc.DeleteMeetingAsync(id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> SaveMinutesOfMeeting(int id, [FromBody] SaveMinutesDto dto)
        {
            var r = await _svc.SaveMinutesOfMeetingAsync(id, dto.MinutesOfMeeting, dto.RecordingLink ?? string.Empty, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        public class SaveMinutesDto
        {
            public string MinutesOfMeeting { get; set; } = null!;
            public string? RecordingLink { get; set; }
        }

        // ═══════════════════════════════════════════════════════════════════════
        // SUPPORT DESK (TICKETS)
        // ═══════════════════════════════════════════════════════════════════════

        [HttpGet]
        public async Task<IActionResult> GetTickets(
            [FromQuery] string? category, [FromQuery] string? status, [FromQuery] string? priority,
            [FromQuery] string? raisedByUserId, [FromQuery] string? assignedStaffId, [FromQuery] string? keyword)
        {
            var filter = new TicketFilterDto { Category = category, Status = status, Priority = priority, RaisedByUserId = raisedByUserId, AssignedStaffId = assignedStaffId, Keyword = keyword };
            var r = await _svc.GetTicketsAsync(filter);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketById(int id)
        {
            var r = await _svc.GetTicketByIdAsync(id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] SupportTicketDto dto)
        {
            var r = await _svc.CreateTicketAsync(dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost("{ticketId}")]
        public async Task<IActionResult> ReplyToTicket(int ticketId, [FromBody] TicketResponseDto dto)
        {
            var r = await _svc.ReplyToTicketAsync(ticketId, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost("{ticketId}")]
        public async Task<IActionResult> ResolveTicket(int ticketId, [FromBody] ResolveTicketDto dto)
        {
            var r = await _svc.ResolveTicketAsync(ticketId, dto.ResolutionNotes, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        public class ResolveTicketDto
        {
            public string ResolutionNotes { get; set; } = null!;
        }

        // ═══════════════════════════════════════════════════════════════════════
        // QUICK POLLS
        // ═══════════════════════════════════════════════════════════════════════

        [HttpGet]
        public async Task<IActionResult> GetPolls([FromQuery] string userId, [FromQuery] bool? isActive)
        {
            var r = await _svc.GetPollsAsync(userId, isActive);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePoll([FromBody] QuickPollDto dto)
        {
            var r = await _svc.CreatePollAsync(dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> VoteInPoll([FromBody] PollVoteDto dto)
        {
            var r = await _svc.VoteInPollAsync(dto.PollId, dto.UserId, dto.SelectedOption);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{pollId}")]
        public async Task<IActionResult> GetPollResults(int pollId)
        {
            var r = await _svc.GetPollResultsAsync(pollId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // DOCUMENT SHARING
        // ═══════════════════════════════════════════════════════════════════════

        [HttpGet]
        public async Task<IActionResult> GetSharedDocuments(
            [FromQuery] string? targetAudience, [FromQuery] string? fileType, [FromQuery] string? keyword)
        {
            var filter = new DocumentFilterDto { TargetAudience = targetAudience, FileType = fileType, Keyword = keyword };
            var r = await _svc.GetSharedDocumentsAsync(filter);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> ShareDocument([FromBody] SharedDocumentDto dto)
        {
            var r = await _svc.ShareDocumentAsync(dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSharedDocument(int id)
        {
            var r = await _svc.DeleteSharedDocumentAsync(id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> TrackDocumentDownload(int id)
        {
            var r = await _svc.TrackDocumentDownloadAsync(id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // TEMPLATES
        // ═══════════════════════════════════════════════════════════════════════

        [HttpGet]
        public async Task<IActionResult> GetTemplates([FromQuery] string? type, [FromQuery] string? keyword)
        {
            var filter = new TemplateFilterDto { Type = type, Keyword = keyword };
            var r = await _svc.GetTemplatesAsync(filter);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTemplate([FromBody] TemplateDto dto)
        {
            var r = await _svc.CreateTemplateAsync(dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTemplate(int id, [FromBody] TemplateDto dto)
        {
            var r = await _svc.UpdateTemplateAsync(id, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTemplate(int id)
        {
            var r = await _svc.DeleteTemplateAsync(id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // GROUP CHATS
        // ═══════════════════════════════════════════════════════════════════════

        [HttpGet]
        public async Task<IActionResult> GetChatRooms([FromQuery] string userId)
        {
            var r = await _svc.GetChatRoomsAsync(userId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateChatRoom([FromBody] GroupChatRoomDto dto)
        {
            var r = await _svc.CreateChatRoomAsync(dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost("{roomId}")]
        public async Task<IActionResult> AddRoomMember(int roomId, [FromBody] AddMemberDto dto)
        {
            var r = await _svc.AddRoomMemberAsync(roomId, dto.UserId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        public class AddMemberDto
        {
            public string UserId { get; set; } = null!;
        }

        [HttpGet("{roomId}")]
        public async Task<IActionResult> GetGroupMessages(int roomId)
        {
            var r = await _svc.GetGroupMessagesAsync(roomId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> SendGroupMessage([FromBody] GroupChatMessageDto dto)
        {
            var r = await _svc.SendGroupMessageAsync(dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // CENTRAL NOTIFICATIONS
        // ═══════════════════════════════════════════════════════════════════════

        [HttpGet]
        public async Task<IActionResult> GetUserNotifications(
            [FromQuery] string userId, [FromQuery] string? category, [FromQuery] string? priority,
            [FromQuery] bool? isRead, [FromQuery] bool? isStarred, [FromQuery] bool? isArchived)
        {
            var filter = new NotificationFilterDto { Category = category, Priority = priority, IsRead = isRead, IsStarred = isStarred, IsArchived = isArchived };
            var r = await _svc.GetUserNotificationsAsync(userId, filter);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> MarkNotificationRead(int id)
        {
            var r = await _svc.MarkNotificationReadAsync(id, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ToggleNotificationStar(int id)
        {
            var r = await _svc.ToggleNotificationStarAsync(id, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ToggleNotificationArchive(int id)
        {
            var r = await _svc.ToggleNotificationArchiveAsync(id, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        // ═══════════════════════════════════════════════════════════════════════
        // DASHBOARD AND AI FEATURES
        // ═══════════════════════════════════════════════════════════════════════

        [HttpGet]
        public async Task<IActionResult> GetCommunicationDashboardStats()
        {
            var r = await _svc.GetCommunicationDashboardStatsAsync();
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> GetAnalyticsChartData()
        {
            var r = await _svc.GetAnalyticsChartDataAsync();
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> GenerateAISmartMessage([FromQuery] string prompt, [FromQuery] string channelType)
        {
            var r = await _svc.GenerateAISmartMessageAsync(prompt, channelType);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> AnalyzeSentiment([FromQuery] string text)
        {
            var r = await _svc.AnalyzeSentimentAsync(text);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }
    }
}
