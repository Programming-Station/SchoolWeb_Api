using System.ComponentModel.DataAnnotations;
using School.Models.CustomeVailidation;

namespace School.Models
{
    public class DropDownModel
    {
        [Required]
        [NoScript]
        public string Table { get; set; }
        [NoScript]
        public string? SearchParams { get; set; }
        public int? ParentId { get; set; }
    }
}

