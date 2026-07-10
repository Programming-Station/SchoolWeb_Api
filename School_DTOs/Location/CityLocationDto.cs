namespace School_DTOs.Location
{
    public class CityLocationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? CityCode { get; set; }
        public string? Description { get; set; }
        public int StateId { get; set; }
        public string? StateName { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class CreateCityLocationDto
    {
        public string Name { get; set; } = null!;
        public string? CityCode { get; set; }
        public string? Description { get; set; }
        public int StateId { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateCityLocationDto : CreateCityLocationDto
    {
        public int Id { get; set; }
    }
}
