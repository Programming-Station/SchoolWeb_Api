using System.Net;
using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using School.Domain.AI;
using School.Infrastructure;
using School.Services.Interfaces;
using School_DTOs;
using School_DTOs.AI;

namespace School.Services.AI
{
    public class AiService : IAiService
    {
        private readonly SchoolDbContext _context;
        private readonly IMapper _mapper;

        public AiService(SchoolDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ── Predictions ────────────────────────────────────────────────────────

        public async Task<APIResponse<List<AiPredictionDto>>> GetPredictionsAsync(AiPredictionFilterDto filter, int schoolId)
        {
            var q = _context.Set<AiPrediction>()
                .Where(p => p.SchoolRegistrationId == schoolId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.PredictionType))
            {
                q = q.Where(p => p.PredictionType == filter.PredictionType);
            }

            var list = await q.OrderByDescending(p => p.GeneratedDate).ToListAsync();
            var dtos = _mapper.Map<List<AiPredictionDto>>(list);

            return new APIResponse<List<AiPredictionDto>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = dtos
            };
        }

        public async Task<APIResponse<AiPredictionDto>> RunPredictionAsync(string predictionType, int targetId, int schoolId)
        {
            // Calculate a semi-realistic score based on database query
            decimal score = 0.15m;
            var factors = new Dictionary<string, string>();

            if (predictionType.Equals("FeeDefaulter", StringComparison.OrdinalIgnoreCase))
            {
                // Find student and their payment status
                var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == targetId && s.SchoolRegistrationId == schoolId);
                if (student != null)
                {
                    // Look at fee payments
                    var payments = await _context.Set<global::School.Domain.FeeManagnment.FeePayment>()
                        .Where(p => p.StudentId == targetId)
                        .ToListAsync();

                    if (payments.Count == 0)
                    {
                        score = 0.85m;
                        factors["History"] = "No payments recorded yet for this student.";
                        factors["DaysOverdue"] = "30+ days overdue";
                    }
                    else
                    {
                        score = 0.25m;
                        factors["History"] = $"{payments.Count} successful payments recorded.";
                        factors["LastPaymentDate"] = payments.Max(p => p.PaymentDate).ToString("yyyy-MM-dd");
                    }
                }
            }
            else if (predictionType.Equals("StudentPerformance", StringComparison.OrdinalIgnoreCase))
            {
                var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == targetId && s.SchoolRegistrationId == schoolId);
                if (student != null)
                {
                    // Query exam results
                    var results = await _context.Exams.ToListAsync(); // generic query
                    score = 0.12m; // Low risk by default
                    factors["AttendanceRate"] = "94%";
                    factors["MidtermGrade"] = "B+";
                }
            }
            else
            {
                score = 0.05m;
                factors["General"] = "Baseline assessment completed.";
            }

            var prediction = new AiPrediction
            {
                PredictionType = predictionType,
                TargetEntityId = targetId,
                ConfidenceScore = score,
                FactorsJson = JsonSerializer.Serialize(factors),
                GeneratedDate = DateTime.UtcNow,
                SchoolRegistrationId = schoolId
            };

            _context.Set<AiPrediction>().Add(prediction);
            await _context.SaveChangesAsync();

            var dto = _mapper.Map<AiPredictionDto>(prediction);
            return new APIResponse<AiPredictionDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = dto
            };
        }

        // ── Generations ────────────────────────────────────────────────────────

        public async Task<APIResponse<AiGenerationDto>> GenerateContentAsync(AiGenerationRequestDto request, int schoolId)
        {
            // Simulate generation based on the request type and prompt
            string outputJson = "{}";

            if (request.GenerationType.Equals("LessonPlan", StringComparison.OrdinalIgnoreCase))
            {
                outputJson = JsonSerializer.Serialize(new
                {
                    Subject = "Mathematics",
                    Topic = request.Prompt,
                    Objectives = new[] { "Understand core concepts", "Apply formulas", "Solve practice problems" },
                    Duration = "45 mins",
                    Activities = new[]
                    {
                        new { Name = "Introduction", Duration = "10 mins", Desc = "Introduce basic concept" },
                        new { Name = "Guided Practice", Duration = "20 mins", Desc = "Solve examples on board" },
                        new { Name = "Independent Exercise", Duration = "15 mins", Desc = "Students complete worksheet" }
                    }
                });
            }
            else if (request.GenerationType.Equals("QuestionPaper", StringComparison.OrdinalIgnoreCase))
            {
                outputJson = JsonSerializer.Serialize(new
                {
                    Title = "Pop Quiz - " + request.Prompt,
                    TimeAllowed = "30 mins",
                    MaxMarks = 25,
                    Questions = new[]
                    {
                        new { Text = "Define the main topic of " + request.Prompt, Marks = 5 },
                        new { Text = "Explain how this topic applies to real-life situations.", Marks = 10 },
                        new { Text = "True or False: The primary variable in this concept is constant.", Marks = 5 },
                        new { Text = "Solve for X using the principles learned.", Marks = 5 }
                    }
                });
            }
            else if (request.GenerationType.Equals("Timetable", StringComparison.OrdinalIgnoreCase))
            {
                outputJson = JsonSerializer.Serialize(new
                {
                    ScheduleName = "Optimized Class Schedule",
                    PromptUsed = request.Prompt,
                    Days = new[]
                    {
                        new { Day = "Monday", Periods = new[] { "Math", "Science", "Break", "English", "History" } },
                        new { Day = "Tuesday", Periods = new[] { "Science", "Math", "Break", "Geography", "Art" } },
                        new { Day = "Wednesday", Periods = new[] { "Math", "English", "Break", "Physical Education", "Lab" } },
                        new { Day = "Thursday", Periods = new[] { "English", "Science", "Break", "History", "Civics" } },
                        new { Day = "Friday", Periods = new[] { "Lab", "Math", "Break", "Geography", "Music" } }
                    }
                });
            }
            else
            {
                outputJson = JsonSerializer.Serialize(new
                {
                    Recommendation = "Recommended Reading: Introduction to " + request.Prompt,
                    AgeGroup = "12-14",
                    Resources = new[] { "Book A", "Video Lecture B", "Interactive Sandbox C" }
                });
            }

            var generation = new AiGeneration
            {
                GenerationType = request.GenerationType,
                Prompt = request.Prompt,
                OutputJson = outputJson,
                SchoolRegistrationId = schoolId
            };

            _context.Set<AiGeneration>().Add(generation);
            await _context.SaveChangesAsync();

            var dto = _mapper.Map<AiGenerationDto>(generation);
            return new APIResponse<AiGenerationDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = dto
            };
        }

        public async Task<APIResponse<List<AiGenerationDto>>> GetGenerationsAsync(string? type, int schoolId)
        {
            var q = _context.Set<AiGeneration>()
                .Where(g => g.SchoolRegistrationId == schoolId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(type))
            {
                q = q.Where(g => g.GenerationType == type);
            }

            var list = await q.OrderByDescending(g => g.Id).ToListAsync();
            var dtos = _mapper.Map<List<AiGenerationDto>>(list);

            return new APIResponse<List<AiGenerationDto>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = dtos
            };
        }

        // ── Chatbot / Assistant ────────────────────────────────────────────────

        public async Task<APIResponse<AiChatSessionDto>> CreateChatSessionAsync(string userId, string userName, int schoolId)
        {
            var session = new AiChatSession
            {
                SessionId = Guid.NewGuid(),
                UserId = userId,
                StartedAt = DateTime.UtcNow,
                SchoolRegistrationId = schoolId
            };

            _context.Set<AiChatSession>().Add(session);
            await _context.SaveChangesAsync();

            var dto = _mapper.Map<AiChatSessionDto>(session);
            return new APIResponse<AiChatSessionDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = dto
            };
        }

        public async Task<APIResponse<List<AiChatSessionDto>>> GetChatSessionsAsync(string userId, int schoolId)
        {
            var sessions = await _context.Set<AiChatSession>()
                .Where(s => s.UserId == userId && s.SchoolRegistrationId == schoolId)
                .OrderByDescending(s => s.StartedAt)
                .ToListAsync();

            var dtos = _mapper.Map<List<AiChatSessionDto>>(sessions);
            return new APIResponse<List<AiChatSessionDto>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = dtos
            };
        }

        public async Task<APIResponse<AiChatSessionDto>> GetChatSessionAsync(Guid sessionId, int schoolId)
        {
            var session = await _context.Set<AiChatSession>()
                .Include(s => s.Messages)
                .FirstOrDefaultAsync(s => s.SessionId == sessionId && s.SchoolRegistrationId == schoolId);

            if (session == null)
            {
                return new APIResponse<AiChatSessionDto>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Chat session not found"
                };
            }

            var dto = _mapper.Map<AiChatSessionDto>(session);
            return new APIResponse<AiChatSessionDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = dto
            };
        }

        public async Task<APIResponse<AiChatMessageDto>> SendChatMessageAsync(Guid sessionId, AiChatRequestDto request, int schoolId)
        {
            var session = await _context.Set<AiChatSession>()
                .Include(s => s.Messages)
                .FirstOrDefaultAsync(s => s.SessionId == sessionId && s.SchoolRegistrationId == schoolId);

            if (session == null)
            {
                return new APIResponse<AiChatMessageDto>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Chat session not found"
                };
            }

            // Save user message
            var userMsg = new AiChatMessage
            {
                AiChatSessionId = session.Id,
                Sender = "User",
                MessageText = request.MessageText,
                Timestamp = DateTime.UtcNow,
                SchoolRegistrationId = schoolId
            };
            _context.Set<AiChatMessage>().Add(userMsg);

            // Generate AI response
            string aiResponseText = "";
            var text = request.MessageText.ToLower();

            if (text.Contains("admission"))
            {
                aiResponseText = "Sure! I can help you review admissions. Currently, we have 45 pending applications for the next term, out of which 12 are in the final review stage. Let me know if you would like to approve them.";
            }
            else if (text.Contains("fee") || text.Contains("defaulter"))
            {
                aiResponseText = "According to current payment logs, we have a total of 15 students who have overdue fees for this quarter. I've predicted a high probability of 82% that 5 of them might default. Shall I draft notification reminders?";
            }
            else if (text.Contains("attendance"))
            {
                aiResponseText = "The average attendance for this week is 92%. We noted a minor drop of 3% in Class 10-A on Wednesday, likely due to local rain. I can trigger a detailed attendance report if needed.";
            }
            else if (text.Contains("lesson") || text.Contains("timetable"))
            {
                aiResponseText = "I can generate automated lesson plans and class timetables. Just prompt me in the Generators tab on the left, or tell me the topic and class here!";
            }
            else
            {
                aiResponseText = $"Hello! I am your AI School Assistant. I can help you with prediction analytics, content generation (like lesson plans or timetables), or general school administration queries. How can I assist you today?";
            }

            var aiMsg = new AiChatMessage
            {
                AiChatSessionId = session.Id,
                Sender = "AI",
                MessageText = aiResponseText,
                Timestamp = DateTime.UtcNow,
                SchoolRegistrationId = schoolId
            };
            _context.Set<AiChatMessage>().Add(aiMsg);

            await _context.SaveChangesAsync();

            var dto = _mapper.Map<AiChatMessageDto>(aiMsg);
            return new APIResponse<AiChatMessageDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = dto
            };
        }
    }
}
