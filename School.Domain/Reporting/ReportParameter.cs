using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using School.Domain.School;
using static School.Domain.BaseEntity;

namespace School.Domain.Reporting
{
    /// <summary>Data type for a report parameter field.</summary>
    public enum ParameterDataType
    {
        String = 1,
        Integer = 2,
        Decimal = 3,
        Date = 4,
        DateTime = 5,
        Boolean = 6,
        Enum = 7,         // Values from EnumValues JSON
        EntityLookup = 8  // Values fetched from an API endpoint
    }

    /// <summary>
    /// Defines a dynamic parameter for a report template.
    /// At runtime, the Angular UI builds a parameter dialog from these records.
    /// </summary>
    [Table("ReportParameters", Schema = "Reporting")]
    public class ReportParameter : AuditEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        public int ReportTemplateId { get; set; }

        [ForeignKey(nameof(ReportTemplateId))]
        public virtual ReportTemplate? ReportTemplate { get; set; }

        /// <summary>Parameter name as it appears in the RDLC definition e.g. FromDate.</summary>
        [Required, MaxLength(100)]
        public string ParameterName { get; set; } = string.Empty;

        /// <summary>Human-readable label shown in the UI e.g. From Date.</summary>
        [Required, MaxLength(200)]
        public string DisplayLabel { get; set; } = string.Empty;

        /// <summary>Input data type.</summary>
        public ParameterDataType DataType { get; set; } = ParameterDataType.String;

        /// <summary>Whether the user must supply this parameter before generating.</summary>
        public bool IsRequired { get; set; } = false;

        /// <summary>Default value (always stored as string, cast at runtime).</summary>
        [MaxLength(500)]
        public string? DefaultValue { get; set; }

        /// <summary>
        /// For Enum DataType: JSON array of label/value pairs
        /// e.g. [{"label":"PDF","value":"PDF"},{"label":"Excel","value":"EXCEL"}]
        /// </summary>
        public string? EnumValuesJson { get; set; }

        /// <summary>
        /// For EntityLookup DataType: API endpoint relative URL to fetch options
        /// e.g. /api/class/list
        /// </summary>
        [MaxLength(255)]
        public string? LookupApiEndpoint { get; set; }

        /// <summary>Placeholder text for the input field.</summary>
        [MaxLength(200)]
        public string? PlaceholderText { get; set; }

        /// <summary>Help text shown as tooltip.</summary>
        [MaxLength(500)]
        public string? HelpText { get; set; }

        /// <summary>Display sort order in the parameter dialog.</summary>
        public int SortOrder { get; set; } = 0;

        /// <summary>Optional: this parameter is only shown when another parameter has a specific value.</summary>
        [MaxLength(100)]
        public string? DependsOnParameter { get; set; }

        [MaxLength(200)]
        public string? DependsOnValue { get; set; }

        [Required]
        public int SchoolRegistrationId { get; set; }

        [ForeignKey(nameof(SchoolRegistrationId))]
        public virtual SchoolRegistration SchoolRegistration { get; set; } = null!;
    }
}


