using System;
using System.Collections.Generic;
using School_DTOs.Common;

namespace School_DTOs.Communication.Recipients
{
    public class RecipientDto : BaseDto
    {
        public string RecipientType { get; set; } = null!;
        public string? RecipientCode { get; set; }
        public string FullName { get; set; } = null!;
        public string? DisplayName { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? WhatsAppNumber { get; set; }
        public string? PreferredChannel { get; set; }
        public bool IsVerified { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsActive { get; set; }

        public List<RecipientGroupDto> Groups { get; set; } = new();
        public List<string> Tags { get; set; } = new();
    }
}
