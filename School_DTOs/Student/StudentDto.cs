using System.Text.Json.Serialization;

namespace School_DTOs.Student
{
    public class StudentDto : BaseDto
    {
        public string StudentId { get; set; } = null!;
        public string? EnrollmentNumber { get; set; }
        public string? CourseType { get; set; }
        public string? SchoolCourse { get; set; }
        public string? UniversityCourse { get; set; }
        public string? CourseOpted { get; set; }
        public string Name { get; set; } = null!;
        public string FathersName { get; set; } = null!;
        public string Sex { get; set; } = null!;
        public string? Nationality { get; set; }
        public string? Occupation { get; set; }
        public int? BirthDate { get; set; }
        public int? BirthMonth { get; set; }
        public int? BirthYear { get; set; }
        public string? SchoolCollege { get; set; }
        public string? QualificationDetails { get; set; }
        public string? PostalAddress { get; set; }
        public string? District { get; set; }
        public string? State { get; set; }
        public string? PinCode { get; set; }
        public string? MobileNo1 { get; set; }
        public string? MobileNo2 { get; set; }
        public string? Image { get; set; }
        public int? ClassId { get; set; }
        public string? ClassName { get; set; }
        public string? Section { get; set; }
        public string Status { get; set; } = null!;
        public string? Remarks { get; set; }

        [JsonPropertyName("stdId")]
        public string StdId { get; set; } = null!;

        [JsonPropertyName("gender")]
        public string Gender { get; set; } = null!;

        [JsonPropertyName("class")]
        public string? Class { get; set; }

        [JsonPropertyName("mobileNo")]
        public string? MobileNo { get; set; }

        [JsonPropertyName("totalFee")]
        public decimal TotalFee { get; set; } = 0;

        [JsonPropertyName("dueFee")]
        public decimal DueFee { get; set; } = 0;
    }
}

