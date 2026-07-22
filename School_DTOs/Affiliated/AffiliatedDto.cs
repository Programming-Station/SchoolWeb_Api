namespace School_DTOs.AffiliationCollege
{
    public class AffiliatedDto
    {
        public int Id { get; set; }

        public string CollegeName { get; set; } = string.Empty;
        public string CollegeCode { get; set; } = string.Empty;

        public string? UniversityName { get; set; }
        public string? UniversityCode { get; set; }

        public int StateId { get; set; }
        public string StateName { get; set; } = string.Empty;

        public int CityId { get; set; }
        public string CityName { get; set; } = string.Empty;

        public string? Address { get; set; }
        public string? Pincode { get; set; }
        public string? ContactPerson { get; set; }
        public string? MobileNo { get; set; }
        public string? Email { get; set; }

        public string? ImagePath { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
