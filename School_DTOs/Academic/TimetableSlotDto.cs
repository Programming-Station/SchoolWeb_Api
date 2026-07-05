namespace School_DTOs.Academic
{
    public class TimetableSlotDto{public int Id{get;set;} public int SubjectId{get;set;} public string DayOfWeek{get;set;}=null!; public TimeSpan StartTime{get;set;} public TimeSpan EndTime{get;set;} public string? Room{get;set;} public string? TeacherName{get;set;} public string Status{get;set;}=""active"";}
    public class CreateTimetableSlotDto{public int SubjectId{get;set;} public string DayOfWeek{get;set;}=null!; public TimeSpan StartTime{get;set;} public TimeSpan EndTime{get;set;} public string? Room{get;set;} public string? TeacherName{get;set;} public string Status{get;set;}=""active"";}
    public class UpdateTimetableSlotDto:CreateTimetableSlotDto{public int Id{get;set;}}
}
