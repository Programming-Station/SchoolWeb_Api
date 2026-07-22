using System.ComponentModel.DataAnnotations;

namespace School.Domain
{
    public class Status
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
