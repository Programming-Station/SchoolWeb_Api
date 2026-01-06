namespace School_DTOs.Website
{
    public class TeamMemberDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public string Qualification { get; set; } = null!;
        public string Experience { get; set; } = null!;
        public string? ProfilePhotoUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
