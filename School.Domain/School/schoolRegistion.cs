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

        public string Address { get; set; }
        public string Pincode { get; set; }

        public int StateId { get; set; }
        public virtual State State { get; set; }

        public int CityId { get; set; }
        public virtual City City { get; set; }

        public string Logo { get; set; }

        public bool IsActive { get; set; } = true;

    }
}
