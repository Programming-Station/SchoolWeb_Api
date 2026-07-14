using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace School.Models.School
{
    public class SchoolRegistrationModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "School Name is required.")]
        [StringLength(200)]
        public string SchoolName { get; set; }

        [StringLength(50)]
        public string? SchoolCode { get; set; }

        [Range(1800, 2100)]
        public int EstablishedYear { get; set; }

        [Required]
        [StringLength(500)]
        public string Address { get; set; }

        [Required]
        [StringLength(10)]
        public string Pincode { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        public int CountryId { get; set; }

        public string? CountryName { get; set; }

        [Required]
        public int StateId { get; set; }

        public string? StateName { get; set; }

        [Required]
        public int CityId { get; set; }

        public string? CityName { get; set; }

        public string? Logo { get; set; }

        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        public string? ContactPersonName { get; set; }

        [StringLength(20)]
        public string? AlternatePhoneNumber { get; set; }

        [StringLength(500)]
        public string? WebsiteUrl { get; set; }

        [StringLength(50)]
        public string ApprovalStatus { get; set; } = "Pending";

        [StringLength(100)]
        public string? SubDomain { get; set; }

        public int? MaxStudentsAllowed { get; set; }

        public int? AffiliationBoardId { get; set; }

        [StringLength(100)]
        public string? AffiliationNumber { get; set; }

        public int? SchoolTypeId { get; set; }

        [StringLength(50)]
        public string? GSTNumber { get; set; }

        [StringLength(50)]
        public string? PANNumber { get; set; }

        [StringLength(100)]
        public string? ContactPersonRole { get; set; }
    }
}
