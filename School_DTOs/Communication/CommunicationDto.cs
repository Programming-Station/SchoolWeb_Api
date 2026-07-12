using System;
using System.Collections.Generic;

namespace School_DTOs.Communication
{
    // ── Filter DTOs ─────────────────────────────────────────────────────────────

    public class NoticeBoardFilterDto
    {
        public string? TargetAudience { get; set; }
        public bool? IsPinned { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class CircularFilterDto
    {
        public string? TargetAudience { get; set; }
        public string? Keyword { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class GatewayLogFilterDto
    {
        public string? RecipientNo { get; set; }     // phone / userId
        public string? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class SurveyFilterDto
    {
        public string? TargetAudience { get; set; }
        public bool? IsActive { get; set; }
        public string? Keyword { get; set; }
    }

    // ── Notice Board ─────────────────────────────────────────────────────────────

    public class NoticeBoardDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string TargetAudience { get; set; } = "All";
        public DateTime PublishedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsPinned { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    // ── Circular ──────────────────────────────────────────────────────────────────

    public class CircularDto
    {
        public int Id { get; set; }
        public string CircularNo { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? AttachmentPath { get; set; }
        public string TargetAudience { get; set; } = "All";
        public DateTime PublishDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    // ── SMS Gateway ───────────────────────────────────────────────────────────────

    public class SmsLogDto
    {
        public int Id { get; set; }
        public string RecipientNo { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string SentStatus { get; set; } = "Pending";
        public DateTime SentDate { get; set; }
        public string? ProviderResponse { get; set; }
    }

    // ── WhatsApp Gateway ──────────────────────────────────────────────────────────

    public class WhatsAppLogDto
    {
        public int Id { get; set; }
        public string RecipientPhone { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string Status { get; set; } = "Pending";
        public DateTime SentDate { get; set; }
    }

    // ── Push Notification ─────────────────────────────────────────────────────────

    public class PushNotificationDto
    {
        public int Id { get; set; }
        public string RecipientUserId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        public bool IsRead { get; set; }
        public DateTime SentDate { get; set; }
    }

    // ── Parent-Teacher Chat ───────────────────────────────────────────────────────

    public class ParentTeacherChatDto
    {
        public int Id { get; set; }
        public string SenderUserId { get; set; } = null!;
        public string SenderUserName { get; set; } = string.Empty;
        public string ReceiverUserId { get; set; } = null!;
        public string ReceiverUserName { get; set; } = string.Empty;
        public string MessageContent { get; set; } = null!;
        public DateTime SentTime { get; set; }
        public bool IsRead { get; set; }
    }

    // ── Survey / Feedback ─────────────────────────────────────────────────────────

    public class SurveyQuestionDto
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public string QuestionText { get; set; } = null!;
        public string QuestionType { get; set; } = "Text"; // Text, Rating, SingleChoice, YesNo
        public string? OptionsJson { get; set; }
    }

    public class FeedbackSurveyDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string TargetAudience { get; set; } = "All";
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public List<SurveyQuestionDto> Questions { get; set; } = new();
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class SurveyResponseDto
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public string RespondentUserId { get; set; } = null!;
        public string RespondentName { get; set; } = string.Empty;
        public string AnswersJson { get; set; } = null!;
        public DateTime SubmittedDate { get; set; }
    }
}
