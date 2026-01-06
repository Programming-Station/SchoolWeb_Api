using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
