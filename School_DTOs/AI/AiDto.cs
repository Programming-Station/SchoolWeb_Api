using System;
using System.Collections.Generic;

namespace School_DTOs.AI
{
    public class AiPredictionDto : BaseDto
    {
        public string PredictionType { get; set; } = null!;
        public int TargetEntityId { get; set; }
        public string TargetEntityName { get; set; } = null!; // E.g., student name or code
        public decimal ConfidenceScore { get; set; }
        public string? FactorsJson { get; set; }
        public DateTime GeneratedDate { get; set; }
        public int SchoolRegistrationId { get; set; }
    }

    public class AiGenerationDto : BaseDto
    {
        public string GenerationType { get; set; } = null!;
        public string Prompt { get; set; } = null!;
        public string OutputJson { get; set; } = null!;
        public int SchoolRegistrationId { get; set; }
    }

    public class AiChatSessionDto : BaseDto
    {
        public Guid SessionId { get; set; }
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public DateTime StartedAt { get; set; }
        public int SchoolRegistrationId { get; set; }
        public List<AiChatMessageDto> Messages { get; set; } = new();
    }

    public class AiChatMessageDto : BaseDto
    {
        public int AiChatSessionId { get; set; }
        public string Sender { get; set; } = null!; // User, AI
        public string MessageText { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public int SchoolRegistrationId { get; set; }
    }

    public class AiPredictionFilterDto
    {
        public string? PredictionType { get; set; }
    }

    public class AiGenerationRequestDto
    {
        public string GenerationType { get; set; } = null!; // Timetable, LessonPlan, QuestionPaper, LibraryRec
        public string Prompt { get; set; } = null!;
        public string? ExtraParametersJson { get; set; }
    }

    public class AiChatRequestDto
    {
        public string MessageText { get; set; } = null!;
    }
}
