using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace School_DTOs.BloodGroup
{
    public class BloodGroupDtos
    {
        public int Id { get; set; }
        public string BloodGroup { get; set; } = null!;
        
        public int Capacity { get; set; }
        public int CurrentStrength { get; set; } = 0;
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
