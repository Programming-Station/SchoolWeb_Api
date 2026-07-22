using School_DTOs.City;

namespace School_DTOs.State
{
    public class StateDto : BaseDto
    {
        public string Name { get; set; } = null!;
        public string? StateCode { get; set; }
        public List<CityDto>? Cities { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
