namespace School_DTOs.Website
{
    public class SchoolRegistrationDto
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public string? StatusName { get; set; } 

        // Basic Information
        public string SchoolName { get; set; } = null!;
        public string SchoolType { get; set; } = null!;
        public int EstablishmentYear { get; set; }
        public string PrincipalName { get; set; } = null!;
        public string PrincipalEmail { get; set; } = null!;

        // Contact Information
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string Pincode { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? AlternatePhone { get; set; }

        // School Details
        public string BoardAffiliation { get; set; } = null!;
        public string? GovernmentRegistrationNumber { get; set; }
        public string? SchoolWebsite { get; set; }
        public string? RegistrationNumber { get; set; }

        // Additional Information
        public FacilitiesDto Facilities { get; set; } = new FacilitiesDto();
        public string? Description { get; set; }
        public bool TermsAccepted { get; set; }

        // Approval fields
        public DateTime? ApprovedAt { get; set; }
        public string? ApprovedBy { get; set; }
        public string? RejectionReason { get; set; }

        // Tracking
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }

        // Audit fields
        public string? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class FacilitiesDto
    {
        public bool Library { get; set; }
        public bool Laboratory { get; set; }
        public bool ComputerLab { get; set; }
        public bool Sports { get; set; }
        public bool Auditorium { get; set; }
        public bool Canteen { get; set; }
        public bool Transport { get; set; }
        public bool Hostel { get; set; }
    }
}
