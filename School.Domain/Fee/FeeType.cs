
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static School.Domain.BaseEntity;

namespace School.Domain.FeeManagnment
{
    public class FeeType : AuditEntity<int>
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string description { get; set; }
        public bool IsActive { get; set; } = true;
        public int SchoolId { get; set; }
      


    }
}
