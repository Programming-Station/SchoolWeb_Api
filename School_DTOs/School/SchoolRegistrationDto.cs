using System;
using System.Collections.Generic;
using System.Text;

namespace School_DTOs.School
{
    public class SchoolRegistrationDto
    {
        public int Id { get; set; }

        public string SchoolName { get; set; }

        public string SchoolCode { get; set; }

        public int EstablishedYear { get; set; }

        public string Address { get; set; }

        public string Pincode { get; set; }

        public int CountryId { get; set; }

        public string CountryName { get; set; }

        public int StateId { get; set; }

        public string StateName { get; set; }

        public int CityId { get; set; }

        public string CityName { get; set; }

        public string Logo { get; set; }

        public bool IsActive { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string ContactPersonName { get; set; }

        public string? AlternatePhoneNumber { get; set; }

        public string? WebsiteUrl { get; set; }

        public string ApprovalStatus { get; set; }

        public string? SubDomain { get; set; }

        public int? MaxStudentsAllowed { get; set; }

        public int? AffiliationBoardId { get; set; }

        public string? AffiliationNumber { get; set; }

        public int? SchoolTypeId { get; set; }

        public string? GSTNumber { get; set; }

        public string? PANNumber { get; set; }

        public string? ContactPersonRole { get; set; }

        public global::School_DTOs.School.SchoolProfileSettingDto? SchoolProfileSetting { get; set; }
        public ICollection<global::School_DTOs.School.SchoolSubscriptionDto> SchoolSubscriptions { get; set; } = new List<global::School_DTOs.School.SchoolSubscriptionDto>();
    }
}
