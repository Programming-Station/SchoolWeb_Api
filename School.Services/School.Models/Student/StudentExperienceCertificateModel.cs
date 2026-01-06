using School.Models.CustomeVailidation;
using System.ComponentModel.DataAnnotations;

namespace School.Models.Student
{
    public class StudentExperienceCertificateModel
    {
        public int Id { get; set; }
        [MaxLength(200)]
        [NoScript]
        public string? Experience { get; set; }
        [MaxLength(200)]
        [NoScript]
        public string? HospitalLabName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        [MaxLength(50)]
        public string? TotalDuration { get; set; }
        [MaxLength(2000)]
        public string? Certificate { get; set; }
    }
}

