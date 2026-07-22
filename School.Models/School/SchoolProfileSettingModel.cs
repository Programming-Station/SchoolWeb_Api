namespace School.Models.School
{
    public class SchoolProfileSettingModel
    {
        public int Id { get; set; }
        public int SchoolRegistrationId { get; set; }

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
    }
}
