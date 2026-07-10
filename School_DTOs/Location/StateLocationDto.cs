namespace School_DTOs.Location
{
    public class StateLocationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? StateCode { get; set; }
        public string? Description { get; set; }
        public int CountryId { get; set; }
        public string? CountryName { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class CreateStateLocationDto
    {
        public string Name { get; set; } = null!;
        public string? StateCode { get; set; }
        public string? Description { get; set; }
        public int CountryId { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateStateLocationDto : CreateStateLocationDto
    {
        public int Id { get; set; }
    }
}
