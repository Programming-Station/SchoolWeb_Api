namespace School_DTOs.Location
{
    public class CountryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? CountryCode { get; set; }
        public string? Currency { get; set; }
        public string? CurrencySymbol { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class CreateCountryDto
    {
        public string Name { get; set; } = null!;
        public string? CountryCode { get; set; }
        public string? Currency { get; set; }
        public string? CurrencySymbol { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateCountryDto : CreateCountryDto
    {
        public int Id { get; set; }
    }
}
