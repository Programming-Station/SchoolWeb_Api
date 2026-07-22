namespace School_DTOs.City
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StateId { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
