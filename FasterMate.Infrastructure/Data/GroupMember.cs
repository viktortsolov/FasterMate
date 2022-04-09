namespace FasterMate.Infrastructure.Data
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class GroupMember
    {
        [Required]
        [MaxLength(36)]
        [ForeignKey(nameof(Group))]
        public string GroupId { get; set; }

        public virtual Group Group { get; set; }

        [Required]
        [MaxLength(36)]
        [ForeignKey(nameof(Profile))]
        public string ProfileId { get; set; }

        public virtual Profile Profile { get; set; }
    }
}
