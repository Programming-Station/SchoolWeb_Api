using System.Collections.Generic;
using System.Threading.Tasks;
using School_DTOs;
using School_DTOs.Communication;

namespace School.Services.Interfaces
{
    public interface ICommunicationService
    {
        // ── Notice Board ─────────────────────────────────────────────────────────
        Task<APIResponse<IEnumerable<NoticeBoardDto>>> GetNoticesAsync(NoticeBoardFilterDto? filter = null);
        Task<APIResponse<NoticeBoardDto>> GetNoticeByIdAsync(int id);
        Task<APIResponse<NoticeBoardDto>> CreateNoticeAsync(NoticeBoardDto dto, string userName);
        Task<APIResponse<NoticeBoardDto>> UpdateNoticeAsync(int id, NoticeBoardDto dto, string userName);
        Task<APIResponse<bool>> DeleteNoticeAsync(int id);

        // ── Circulars ────────────────────────────────────────────────────────────
        Task<APIResponse<IEnumerable<CircularDto>>> GetCircularsAsync(CircularFilterDto? filter = null);
        Task<APIResponse<CircularDto>> GetCircularByIdAsync(int id);
        Task<APIResponse<CircularDto>> CreateCircularAsync(CircularDto dto, string userName);
        Task<APIResponse<CircularDto>> UpdateCircularAsync(int id, CircularDto dto, string userName);
        Task<APIResponse<bool>> DeleteCircularAsync(int id);

        // ── SMS Gateway ──────────────────────────────────────────────────────────
        Task<APIResponse<IEnumerable<SmsLogDto>>> GetSmsLogsAsync(GatewayLogFilterDto? filter = null);
        Task<APIResponse<SmsLogDto>> GetSmsLogByIdAsync(int id);
        Task<APIResponse<bool>> SendSmsAsync(SmsLogDto dto, string userName);

        // ── WhatsApp Gateway ─────────────────────────────────────────────────────
        Task<APIResponse<IEnumerable<WhatsAppLogDto>>> GetWhatsAppLogsAsync(GatewayLogFilterDto? filter = null);
        Task<APIResponse<WhatsAppLogDto>> GetWhatsAppLogByIdAsync(int id);
        Task<APIResponse<bool>> SendWhatsAppAsync(WhatsAppLogDto dto, string userName);

        // ── Push Notifications ───────────────────────────────────────────────────
        Task<APIResponse<IEnumerable<PushNotificationDto>>> GetPushNotificationsAsync(string userId, GatewayLogFilterDto? filter = null);
        Task<APIResponse<PushNotificationDto>> GetPushNotificationByIdAsync(int id);
        Task<APIResponse<bool>> SendPushNotificationAsync(PushNotificationDto dto, string userName);
        Task<APIResponse<bool>> MarkPushNotificationReadAsync(int id, string userName);

        // ── Parent-Teacher Chat ──────────────────────────────────────────────────
        Task<APIResponse<IEnumerable<ParentTeacherChatDto>>> GetChatHistoryAsync(string senderId, string receiverId);
        Task<APIResponse<ParentTeacherChatDto>> GetChatMessageByIdAsync(int id);
        Task<APIResponse<bool>> SendChatMessageAsync(ParentTeacherChatDto dto, string userName);
        Task<APIResponse<bool>> MarkChatMessageReadAsync(int id, string userName);
        Task<APIResponse<bool>> DeleteChatMessageAsync(int id);

        // ── Feedback & Surveys ───────────────────────────────────────────────────
        Task<APIResponse<IEnumerable<FeedbackSurveyDto>>> GetSurveysAsync(SurveyFilterDto? filter = null);
        Task<APIResponse<FeedbackSurveyDto>> GetSurveyByIdAsync(int id);
        Task<APIResponse<FeedbackSurveyDto>> CreateSurveyAsync(FeedbackSurveyDto dto, string userName);
        Task<APIResponse<FeedbackSurveyDto>> UpdateSurveyAsync(int id, FeedbackSurveyDto dto, string userName);
        Task<APIResponse<bool>> DeleteSurveyAsync(int id);
        Task<APIResponse<bool>> SubmitSurveyResponseAsync(SurveyResponseDto dto, string userName);
        Task<APIResponse<IEnumerable<SurveyResponseDto>>> GetSurveyResponsesAsync(int surveyId);

        // ── Announcements ────────────────────────────────────────────────────────
        Task<APIResponse<IEnumerable<AnnouncementDto>>> GetAnnouncementsAsync(AnnouncementFilterDto? filter = null);
        Task<APIResponse<AnnouncementDto>> GetAnnouncementByIdAsync(int id);
        Task<APIResponse<AnnouncementDto>> CreateAnnouncementAsync(AnnouncementDto dto, string userName);
        Task<APIResponse<AnnouncementDto>> UpdateAnnouncementAsync(int id, AnnouncementDto dto, string userName);
        Task<APIResponse<bool>> DeleteAnnouncementAsync(int id);

        // ── Meetings & Virtual Classes ───────────────────────────────────────────
        Task<APIResponse<IEnumerable<MeetingDto>>> GetMeetingsAsync(MeetingFilterDto? filter = null);
        Task<APIResponse<MeetingDto>> GetMeetingByIdAsync(int id);
        Task<APIResponse<MeetingDto>> CreateMeetingAsync(MeetingDto dto, string userName);
        Task<APIResponse<MeetingDto>> UpdateMeetingAsync(int id, MeetingDto dto, string userName);
        Task<APIResponse<bool>> DeleteMeetingAsync(int id);
        Task<APIResponse<bool>> SaveMinutesOfMeetingAsync(int id, string minutes, string recordingLink, string userName);

        // ── Support Desk (Tickets) ────────────────────────────────────────────────
        Task<APIResponse<IEnumerable<SupportTicketDto>>> GetTicketsAsync(TicketFilterDto? filter = null);
        Task<APIResponse<SupportTicketDto>> GetTicketByIdAsync(int id);
        Task<APIResponse<SupportTicketDto>> CreateTicketAsync(SupportTicketDto dto, string userName);
        Task<APIResponse<SupportTicketDto>> ReplyToTicketAsync(int ticketId, TicketResponseDto dto, string userName);
        Task<APIResponse<SupportTicketDto>> ResolveTicketAsync(int ticketId, string resolutionNotes, string userName);

        // ── Quick Polls ───────────────────────────────────────────────────────────
        Task<APIResponse<IEnumerable<QuickPollDto>>> GetPollsAsync(string userId, bool? isActive = null);
        Task<APIResponse<QuickPollDto>> CreatePollAsync(QuickPollDto dto, string userName);
        Task<APIResponse<bool>> VoteInPollAsync(int pollId, string userId, string selectedOption);
        Task<APIResponse<IEnumerable<PollResultDto>>> GetPollResultsAsync(int pollId);

        // ── Document Sharing ──────────────────────────────────────────────────────
        Task<APIResponse<IEnumerable<SharedDocumentDto>>> GetSharedDocumentsAsync(DocumentFilterDto? filter = null);
        Task<APIResponse<SharedDocumentDto>> ShareDocumentAsync(SharedDocumentDto dto, string userName);
        Task<APIResponse<bool>> DeleteSharedDocumentAsync(int id);
        Task<APIResponse<bool>> TrackDocumentDownloadAsync(int id);

        // ── Communication Templates ───────────────────────────────────────────────
        Task<APIResponse<IEnumerable<TemplateDto>>> GetTemplatesAsync(TemplateFilterDto? filter = null);
        Task<APIResponse<TemplateDto>> CreateTemplateAsync(TemplateDto dto, string userName);
        Task<APIResponse<TemplateDto>> UpdateTemplateAsync(int id, TemplateDto dto, string userName);
        Task<APIResponse<bool>> DeleteTemplateAsync(int id);

        // ── Group Chats & Channels ───────────────────────────────────────────────
        Task<APIResponse<IEnumerable<GroupChatRoomDto>>> GetChatRoomsAsync(string userId);
        Task<APIResponse<GroupChatRoomDto>> CreateChatRoomAsync(GroupChatRoomDto dto, string userName);
        Task<APIResponse<bool>> AddRoomMemberAsync(int roomId, string userId);
        Task<APIResponse<IEnumerable<GroupChatMessageDto>>> GetGroupMessagesAsync(int roomId);
        Task<APIResponse<GroupChatMessageDto>> SendGroupMessageAsync(GroupChatMessageDto dto, string userName);

        // ── Centralized Notification Center ────────────────────────────────────────
        Task<APIResponse<IEnumerable<CentralNotificationDto>>> GetUserNotificationsAsync(string userId, NotificationFilterDto? filter = null);
        Task<APIResponse<bool>> MarkNotificationReadAsync(int id, string userName);
        Task<APIResponse<bool>> ToggleNotificationStarAsync(int id, string userName);
        Task<APIResponse<bool>> ToggleNotificationArchiveAsync(int id, string userName);
        Task<APIResponse<bool>> SendSystemNotificationAsync(string recipientUserId, string title, string body, string category, string priority, string? actionUrl);

        // ── Dashboard Statistics & AI Helpers ─────────────────────────────────────
        Task<APIResponse<CommunicationDashboardStatsDto>> GetCommunicationDashboardStatsAsync();
        Task<APIResponse<IEnumerable<AnalyticsChartDataDto>>> GetAnalyticsChartDataAsync();
        Task<APIResponse<string>> GenerateAISmartMessageAsync(string prompt, string channelType);
        Task<APIResponse<string>> AnalyzeSentimentAsync(string text);
    }
}
