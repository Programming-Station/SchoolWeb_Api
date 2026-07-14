using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;
using School.Domain.Location;
using static School.Domain.BaseEntity;

namespace School.Domain.School
{
    [Table("SchoolRegistrations", Schema = "School")]
    public class SchoolRegistration : AuditEntity<int>
    {
        [Key]
        public int Id { get; set; }
        
        public string SchoolName { get; set; } = string.Empty;
        public string SchoolCode { get; set; } = string.Empty;
        public int EstablishedYear { get; set; }

        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? AlternatePhoneNumber { get; set; }
        public string? WebsiteUrl { get; set; }
        
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        public string ApprovalStatus { get; set; } = "Pending"; // Pending, Approved, Rejected

        public string? SubDomain { get; set; }
        public int? MaxStudentsAllowed { get; set; }

        public int? AffiliationBoardId { get; set; }
        public virtual AffiliationBoard? AffiliationBoard { get; set; }
        
        public string? AffiliationNumber { get; set; }
        
        public int? SchoolTypeId { get; set; }
        public virtual SchoolType? SchoolType { get; set; }

        public string? GSTNumber { get; set; }
        public string? PANNumber { get; set; }
        
        public string? ContactPersonName { get; set; }
        public string? ContactPersonRole { get; set; }

        public string? Address { get; set; }
        public string? Pincode { get; set; }

        public int CountryId { get; set; }
        public virtual Country Country { get; set; } = null!;

        public int StateId { get; set; }
        public virtual State State { get; set; } = null!;

        public int CityId { get; set; }
        public virtual City City { get; set; } = null!;

        public string? Logo { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual SchoolProfileSetting? SchoolProfileSetting { get; set; }
        public virtual ICollection<SchoolSubscription> SchoolSubscriptions { get; set; } = new List<SchoolSubscription>();
    }
}
