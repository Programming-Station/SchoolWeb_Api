namespace School_DTOs.Student
{
    public class StudentExperienceCertificateDto
    {
        public int Id { get; set; }
        public int StudentRegistrationId { get; set; }
        public string? Experience { get; set; }
        public string? HospitalLabName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? TotalDuration { get; set; }
        public string? Certificate { get; set; }
    }
}

