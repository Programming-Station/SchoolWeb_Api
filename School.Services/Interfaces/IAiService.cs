using School_DTOs;
using School_DTOs.AI;

namespace School.Services.Interfaces
{
    public interface IAiService
    {
        // Predictions
        Task<APIResponse<List<AiPredictionDto>>> GetPredictionsAsync(AiPredictionFilterDto filter, int schoolId);
        Task<APIResponse<AiPredictionDto>> RunPredictionAsync(string predictionType, int targetId, int schoolId);

        // Generations
        Task<APIResponse<AiGenerationDto>> GenerateContentAsync(AiGenerationRequestDto request, int schoolId);
        Task<APIResponse<List<AiGenerationDto>>> GetGenerationsAsync(string? type, int schoolId);

        // Chatbot / Assistant
        Task<APIResponse<AiChatSessionDto>> CreateChatSessionAsync(string userId, string userName, int schoolId);
        Task<APIResponse<List<AiChatSessionDto>>> GetChatSessionsAsync(string userId, int schoolId);
        Task<APIResponse<AiChatSessionDto>> GetChatSessionAsync(Guid sessionId, int schoolId);
        Task<APIResponse<AiChatMessageDto>> SendChatMessageAsync(Guid sessionId, AiChatRequestDto request, int schoolId);
    }
}
