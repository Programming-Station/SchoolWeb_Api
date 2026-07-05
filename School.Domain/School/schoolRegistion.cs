using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using static School.Domain.BaseEntity;

namespace School.Domain.School
{
    public class schoolRegistion : AuditEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public string SchoolName { get; set; }
        public string SchoolCode { get; set; }
        public int EstablishedYear { get; set; }

        // Address
        public string Address { get; set; }
        public string Pincode { get; set; }

        // State
        public int StateId { get; set; }
        public virtual State State { get; set; }

        // City
        public int CityId { get; set; }
        public virtual City City { get; set; }

        // School Logo
        public string Logo { get; set; }

        // Status
        public bool IsActive { get; set; } = true;

    }
}
