using School.Utilities.Enums;
using System.ComponentModel.DataAnnotations;

namespace School.Models.Student
{
    public class StudentModel
    {
        public int? Id { get; set; }

        [MaxLength(50)]
        public string? StudentId { get; set; }

        [MaxLength(50)]
        public string? EnrollmentNumber { get; set; }

        [MaxLength(20)]
        public CourseType? CourseType { get; set; } // School, UNIVERSITY, or BOTH

        [MaxLength(200)]
        public string? SchoolCourse { get; set; }

        [MaxLength(200)]
        public string? UniversityCourse { get; set; }

        [MaxLength(200)]
        public string? CourseOpted { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Father's name is required")]
        [MaxLength(200, ErrorMessage = "Father's name cannot exceed 200 characters")]
        public string FathersName { get; set; } = null!;

        [Required(ErrorMessage = "Sex is required")]
        [MaxLength(10, ErrorMessage = "Sex cannot exceed 10 characters")]
        public string Sex { get; set; } = null!; // Male, Female

        [MaxLength(100)]
        public string? Nationality { get; set; }

        [MaxLength(100)]
        public string? Occupation { get; set; } 
        public string? DateOfBirth { get; set; }  

        [MaxLength(200)]
        public string? SchoolCollege { get; set; }

        [MaxLength(1000)]
        public string? QualificationDetails { get; set; }

        [MaxLength(500)]
        public string? PostalAddress { get; set; } 
        public int? CityId { get; set; } 
        public int? StateId { get; set; }

        [MaxLength(10)]
        public string? PinCode { get; set; }

        [MaxLength(15)]
        public string? MobileNo1 { get; set; }

        [MaxLength(15)]
        public string? MobileNo2 { get; set; }

        public string? Image { get; set; } // Base64 or file path

        public int? ClassId { get; set; }

        [MaxLength(50)]
        public DefaultStatus? Status { get; set; } // active, inactive, graduated, transferred

        [MaxLength(1000)]
        public string? Remarks { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}

