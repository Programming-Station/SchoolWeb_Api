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

        [Required(ErrorMessage = "School Code is required.")]
        [StringLength(50)]
        public string SchoolCode { get; set; }

        [Range(1800, 2100)]
        public int EstablishedYear { get; set; }

        [Required]
        [StringLength(500)]
        public string Address { get; set; }

        [Required]
        [StringLength(10)]
        public string Pincode { get; set; }

        [Required]
        public int StateId { get; set; }

        public string StateName { get; set; }

        [Required]
        public int CityId { get; set; }

        public string CityName { get; set; }

        public string Logo { get; set; }

        public bool IsActive { get; set; }

    }
}
