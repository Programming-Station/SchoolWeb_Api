using School.Domain.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static School.Domain.BaseEntity;

namespace School.Domain.School
{
    public class schoolOwner : AuditEntity<int>
    {
        [Key]
        public int Id { get; set; }

        public int SchoolRegistrationId { get; set; }
        public virtual schoolRegistion SchoolRegistration { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public string ProfilePhoto { get; set; }

        public int StatusId { get; set; }
        public virtual Status Status { get; set; }

        public bool EmailVerified { get; set; }

        public bool MobileVerified { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public string LastLoginIp { get; set; }

        public int FailedLoginAttempt { get; set; }

        public bool IsLocked { get; set; }
    }
}
