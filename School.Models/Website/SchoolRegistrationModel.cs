using System.ComponentModel.DataAnnotations;

namespace School.Models.Website
{
    public class SchoolRegistrationModel
    {
        public int? Id { get; set; }

        // Basic Information
        [Required(ErrorMessage = "School name is required")]
        [MinLength(3, ErrorMessage = "School name must be at least 3 characters")]
        [MaxLength(200, ErrorMessage = "School name cannot exceed 200 characters")]
        public string SchoolName { get; set; } = null!;

        [Required(ErrorMessage = "School type is required")]
        [MaxLength(50, ErrorMessage = "School type cannot exceed 50 characters")]
        public string SchoolType { get; set; } = null!;

        [Required(ErrorMessage = "Establishment year is required")]
        [Range(1900, 2100, ErrorMessage = "Please enter a valid establishment year")]
        public int EstablishmentYear { get; set; }

        [Required(ErrorMessage = "Principal name is required")]
        [MinLength(2, ErrorMessage = "Principal name must be at least 2 characters")]
        [MaxLength(200, ErrorMessage = "Principal name cannot exceed 200 characters")]
        public string PrincipalName { get; set; } = null!;

        [Required(ErrorMessage = "Principal email is required")]
        [MaxLength(200, ErrorMessage = "Email cannot exceed 200 characters")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string PrincipalEmail { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        [MaxLength(100, ErrorMessage = "Password cannot exceed 100 characters")]
        public string Password { get; set; } = null!;

        // Contact Information
        [Required(ErrorMessage = "Address is required")]
        [MinLength(10, ErrorMessage = "Address must be at least 10 characters")]
        [MaxLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "City is required")]
        [MinLength(2, ErrorMessage = "City must be at least 2 characters")]
        [MaxLength(100, ErrorMessage = "City cannot exceed 100 characters")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "State is required")]
        [MinLength(2, ErrorMessage = "State must be at least 2 characters")]
        [MaxLength(100, ErrorMessage = "State cannot exceed 100 characters")]
        public string State { get; set; } = null!;

        [Required(ErrorMessage = "Pincode is required")]
        [RegularExpression(@"^[0-9]{6}$", ErrorMessage = "Pincode must be 6 digits")]
        public string Pincode { get; set; } = null!;

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must be 10 digits")]
        [MaxLength(15, ErrorMessage = "Phone number cannot exceed 15 characters")]
        public string PhoneNumber { get; set; } = null!;

        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Alternate phone number must be 10 digits")]
        [MaxLength(15, ErrorMessage = "Alternate phone number cannot exceed 15 characters")]
        public string? AlternatePhone { get; set; }

        // School Details
        [Required(ErrorMessage = "Board affiliation is required")]
        [MaxLength(100, ErrorMessage = "Board affiliation cannot exceed 100 characters")]
        public string BoardAffiliation { get; set; } = null!;

        [MaxLength(100, ErrorMessage = "Government registration number cannot exceed 100 characters")]
        public string? GovernmentRegistrationNumber { get; set; }

        [Url(ErrorMessage = "Invalid website URL format")]
        [MaxLength(500, ErrorMessage = "Website URL cannot exceed 500 characters")]
        public string? SchoolWebsite { get; set; }

        // Additional Information - Facilities (as JSON object)
        public FacilitiesModel Facilities { get; set; } = new FacilitiesModel();

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "You must accept the terms and conditions")]
        public bool TermsAccepted { get; set; }

        public int StatusId { get; set; } // Will be set programmatically, default to "Pending" status if not provided

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class FacilitiesModel
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
