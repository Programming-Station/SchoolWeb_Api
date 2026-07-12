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
            [FromQuery] System.DateTime? fromDate, [FromQuery] System.DateTime? toDate)
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
            [FromQuery] System.DateTime? fromDate, [FromQuery] System.DateTime? toDate)
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
            [FromQuery] System.DateTime? fromDate, [FromQuery] System.DateTime? toDate)
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
            [FromQuery] System.DateTime? fromDate, [FromQuery] System.DateTime? toDate)
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
            [FromQuery] System.DateTime? fromDate, [FromQuery] System.DateTime? toDate)
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
    }
}
