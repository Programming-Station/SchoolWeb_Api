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
    }
}
