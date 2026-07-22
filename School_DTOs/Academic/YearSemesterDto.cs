namespace School_DTOs.Academic
{
    public class YearSemesterDto : BaseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int Sequence { get; set; }
        public string Status { get; set; } = "active";
    }
}
