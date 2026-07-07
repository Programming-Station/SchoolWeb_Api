using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Student
{
    public class StudentExperienceCertificate : AuditEntity<int>
    {
        [Key]
        public int Id { get; set; } 
        public int StudentRegistrationId { get; set; }
        public string? Experience { get; set; }
        public string? HospitalLabName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? TotalDuration { get; set; }
        public string? Certificate { get; set; } // Base64 or file path

        [ForeignKey(nameof(StudentRegistrationId))]
        public virtual StudentRegistration? StudentRegistration { get; set; }
    }
}

