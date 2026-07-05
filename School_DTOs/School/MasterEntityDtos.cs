using System.ComponentModel.DataAnnotations;

namespace School_DTOs.School
{
    // AffiliationBoard
    public class AffiliationBoardDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class AffiliationBoardModel
    {
        public int? Id { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        public bool IsActive { get; set; } = true;
    }

    // SchoolType
    public class SchoolTypeDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class SchoolTypeModel
    {
        public int? Id { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        public bool IsActive { get; set; } = true;
    }

    // SchoolMedium
    public class SchoolMediumDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class SchoolMediumModel
    {
        public int? Id { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        public bool IsActive { get; set; } = true;
    }
}
