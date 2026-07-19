using System;
using System.Collections.Generic;
using School_DTOs.Common;

namespace School_DTOs.Communication.Recipients
{
    public class RecipientGroupDto : BaseDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsDynamic { get; set; }
        public int MemberCount { get; set; }
    }
}
