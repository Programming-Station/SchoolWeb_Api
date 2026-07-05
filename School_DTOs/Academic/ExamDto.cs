namespace School_DTOs.Academic
{
    public class ExamDto { public int Id{get;set;} public string Name{get;set;}=null!; public string? ExamType{get;set;} public DateTime StartDate{get;set;} public DateTime EndDate{get;set;} public string Status{get;set;}="Scheduled"; public string? Description{get;set;} }
    public class CreateExamDto { public string Name{get;set;}=null!; public string? ExamType{get;set;} public DateTime StartDate{get;set;} public DateTime EndDate{get;set;} public string Status{get;set;}="Scheduled"; public string? Description{get;set;} }
    public class UpdateExamDto : CreateExamDto { public int Id{get;set;} }
    public class ExamResultDto { public int Id{get;set;} public int ExamId{get;set;} public int StudentId{get;set;} public int SubjectId{get;set;} public decimal MarksObtained{get;set;} public decimal TotalMarks{get;set;} public string? Grade{get;set;} public string Status{get;set;}="Pass"; }
    public class CreateExamResultDto { public int ExamId{get;set;} public int StudentId{get;set;} public int SubjectId{get;set;} public decimal MarksObtained{get;set;} public decimal TotalMarks{get;set;} public string? Grade{get;set;} public string Status{get;set;}="Pass"; }
    public class UpdateExamResultDto : CreateExamResultDto { public int Id{get;set;} }
}
