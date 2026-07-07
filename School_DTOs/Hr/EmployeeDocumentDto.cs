using System;
namespace School_DTOs.Hr
{
    public class EmployeeDocumentDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string DocumentName { get; set; } = null!; public string DocumentType { get; set; } = null!; public string FilePath { get; set; } = null!;
    }

    public class CreateEmployeeDocumentDto
    {
        public int EmployeeId { get; set; }
        public string DocumentName { get; set; } = null!; public string DocumentType { get; set; } = null!; public string FilePath { get; set; } = null!;
    }

    public class UpdateEmployeeDocumentDto : CreateEmployeeDocumentDto
    {
        public int Id { get; set; }
    }
}