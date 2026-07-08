using System;

namespace School_DTOs.Student
{
    public class UpdateAdmissionApplicationStatusDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty; // Submitted, Under Verification, Approved, Rejected, Waiting List, Cancelled, Enrolled
        public string? Remarks { get; set; }
        public string? VerificationNotes { get; set; }
    }
}
