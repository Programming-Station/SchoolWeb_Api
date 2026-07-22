using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School.Domain.BaseEntity;

namespace School.Domain.Communication
{
    [Table("PollVotes", Schema = "Communication")]
    public class PollVote : DeleteEntity<int>, ITenantEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PollId { get; set; }

        [ForeignKey(nameof(PollId))]
        public virtual QuickPoll Poll { get; set; } = null!;

        [Required, MaxLength(450)]
        public string UserId { get; set; } = null!;

        [Required, MaxLength(200)]
        public string SelectedOption { get; set; } = null!;

        public DateTime VotedAt { get; set; } = DateTime.UtcNow;

        public int SchoolRegistrationId { get; set; }
    }
}
