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

    // ── Announcements ─────────────────────────────────────────────────────────────
    public class AnnouncementFilterDto
    {
        public string? Scope { get; set; }
        public string? TargetReferenceId { get; set; }
        public string? Priority { get; set; }
        public string? Keyword { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class AnnouncementDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Scope { get; set; } = "School";
        public string? TargetReferenceId { get; set; }
        public string Priority { get; set; } = "Medium";
        public string? AttachmentPath { get; set; }
        public string? ImagePath { get; set; }
        public bool IsPinned { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    // ── Meetings ──────────────────────────────────────────────────────────────────
    public class MeetingFilterDto
    {
        public string? Platform { get; set; }
        public string? Status { get; set; }
        public string? TargetAudience { get; set; }
        public string? Keyword { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class MeetingDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Platform { get; set; } = "Zoom";
        public string? MeetingLink { get; set; }
        public string? MeetingId { get; set; }
        public string? MeetingPassword { get; set; }
        public string? Agenda { get; set; }
        public string? MinutesOfMeeting { get; set; }
        public string? RecordingLink { get; set; }
        public string Status { get; set; } = "Scheduled";
        public string TargetAudience { get; set; } = "All";
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    // ── Support Desk (Tickets) ────────────────────────────────────────────────────
    public class TicketFilterDto
    {
        public string? Category { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public string? RaisedByUserId { get; set; }
        public string? AssignedStaffId { get; set; }
        public string? Keyword { get; set; }
    }

    public class SupportTicketDto
    {
        public int Id { get; set; }
        public string TicketNumber { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Category { get; set; } = "General";
        public string Status { get; set; } = "Open";
        public string Priority { get; set; } = "Medium";
        public string RaisedByUserId { get; set; } = null!;
        public string RaisedByUserName { get; set; } = string.Empty;
        public string? AssignedStaffId { get; set; }
        public string AssignedStaffName { get; set; } = string.Empty;
        public DateTime? SLAExpiryDate { get; set; }
        public string? ResolutionNotes { get; set; }
        public int? FeedbackRating { get; set; }
        public string? FeedbackComments { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<TicketResponseDto> Responses { get; set; } = new();
    }

    public class TicketResponseDto
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string SenderUserId { get; set; } = null!;
        public string SenderUserName { get; set; } = string.Empty;
        public string Content { get; set; } = null!;
        public bool IsInternalNote { get; set; }
        public string? AttachmentPath { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    // ── Quick Polls ───────────────────────────────────────────────────────────────
    public class QuickPollDto
    {
        public int Id { get; set; }
        public string Question { get; set; } = null!;
        public string OptionsJson { get; set; } = "[]";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public string TargetAudience { get; set; } = "All";
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public List<PollVoteDto> Votes { get; set; } = new();
        public bool UserHasVoted { get; set; }
        public string? UserVotedOption { get; set; }
    }

    public class PollVoteDto
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public string UserId { get; set; } = null!;
        public string SelectedOption { get; set; } = null!;
        public DateTime VotedAt { get; set; }
    }

    public class PollResultDto
    {
        public string Option { get; set; } = null!;
        public int VoteCount { get; set; }
        public double Percentage { get; set; }
    }

    // ── Document Sharing ──────────────────────────────────────────────────────────
    public class DocumentFilterDto
    {
        public string? TargetAudience { get; set; }
        public string? FileType { get; set; }
        public string? Keyword { get; set; }
    }

    public class SharedDocumentDto
    {
        public int Id { get; set; }
        public string FileName { get; set; } = null!;
        public string? Description { get; set; }
        public string FilePath { get; set; } = null!;
        public long FileSize { get; set; }
        public string? FileType { get; set; }
        public string TargetAudience { get; set; } = "All";
        public DateTime? ExpiryDate { get; set; }
        public int DownloadCount { get; set; }
        public bool IsPublicLink { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    // ── Templates ─────────────────────────────────────────────────────────────────
    public class TemplateFilterDto
    {
        public string? Type { get; set; }
        public string? Keyword { get; set; }
    }

    public class TemplateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = "Email";
        public string? SubjectTemplate { get; set; }
        public string BodyTemplate { get; set; } = null!;
        public bool IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    // ── Group Chat & Channels ─────────────────────────────────────────────────────
    public class GroupChatRoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = "Group"; // Group, Department, Class, Broadcast
        public string? TargetReferenceId { get; set; }
        public int MemberCount { get; set; }
        public List<ChatUserDto> Members { get; set; } = new();
        public string? LastMessageContent { get; set; }
        public DateTime? LastMessageTime { get; set; }
    }

    public class GroupChatMessageDto
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string SenderUserId { get; set; } = null!;
        public string SenderUserName { get; set; } = string.Empty;
        public string SenderRoleName { get; set; } = string.Empty;
        public string MessageContent { get; set; } = null!;
        public string? AttachmentPath { get; set; }
        public DateTime SentTime { get; set; }
    }

    public class ChatUserDto
    {
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public bool IsOnline { get; set; }
    }

    // ── Centralized Notifications ─────────────────────────────────────────────────
    public class NotificationFilterDto
    {
        public string? Category { get; set; }
        public string? Priority { get; set; }
        public bool? IsRead { get; set; }
        public bool? IsStarred { get; set; }
        public bool? IsArchived { get; set; }
    }

    public class CentralNotificationDto
    {
        public int Id { get; set; }
        public string RecipientUserId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        public string Category { get; set; } = "General";
        public string Priority { get; set; } = "Medium";
        public string? ActionUrl { get; set; }
        public bool IsRead { get; set; }
        public bool IsStarred { get; set; }
        public bool IsArchived { get; set; }
        public DateTime SentDate { get; set; }
    }

    // ── Dashboard & Analytics ─────────────────────────────────────────────────────
    public class CommunicationDashboardStatsDto
    {
        public int MessagesSentToday { get; set; }
        public int UnreadMessages { get; set; }
        public int EmailsSent { get; set; }
        public int SmsSent { get; set; }
        public int WhatsAppSent { get; set; }
        public int PushNotificationsSent { get; set; }
        public int AnnouncementsCount { get; set; }
        public int CircularsCount { get; set; }
        public int NoticesCount { get; set; }
        public int EventsCount { get; set; }
        public int MeetingsCount { get; set; }
        public int OnlineClassesCount { get; set; }
        public int ActiveSupportTickets { get; set; }
        public int ChatConversationsCount { get; set; }
        public int PendingApprovalsCount { get; set; }
        public int ScheduledMessagesCount { get; set; }
        public int FailedDeliveriesCount { get; set; }
        public double DeliverySuccessRate { get; set; }
    }

    public class AnalyticsChartDataDto
    {
        public List<string> Labels { get; set; } = new();
        public List<int> Values { get; set; } = new();
        public string ChartName { get; set; } = string.Empty;
    }
}
