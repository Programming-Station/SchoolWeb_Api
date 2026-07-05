using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.School
{
    public class SchoolProfileSetting : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int SchoolRegistrationId { get; set; }
        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;

        // Bank Details
        public string? BankAccountName { get; set; }
        public string? BankAccountNumber { get; set; }
        public string? BankIFSCCode { get; set; }
        public string? BankName { get; set; }
        public string? BankBranch { get; set; }

        // Location
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        // Social Links
        public string? FacebookUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? TwitterUrl { get; set; }

        // Other
        public string? Tagline { get; set; }

        public int? PrimaryMediumId { get; set; }
        [ForeignKey(nameof(PrimaryMediumId))]
        public virtual SchoolMedium? PrimaryMedium { get; set; }
    }
}

