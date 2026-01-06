namespace School_DTOs.Website
{
    public class ContactInfoDto
    {
        public int Id { get; set; }
        public string OrganizationName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone1 { get; set; } = null!;
        public string? Phone2 { get; set; }
        public bool IsActive { get; set; }
    }
}
