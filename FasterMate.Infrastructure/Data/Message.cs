namespace FasterMate.Infrastructure.Data
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Message
    {
        [Key]
        [MaxLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Text { get; set; }

        [Required]
        [MaxLength(36)]
        [ForeignKey(nameof(Profile))]
        public string ProfileId { get; set; }

        public virtual Profile Profile { get; set; }
        
        [Required]
        [MaxLength(36)]
        [ForeignKey(nameof(Group))]
        public string GroupId { get; set; }

        public virtual Group Group { get; set; }

        public DateTime CreateOn { get; set; } = DateTime.UtcNow;
    }
}
