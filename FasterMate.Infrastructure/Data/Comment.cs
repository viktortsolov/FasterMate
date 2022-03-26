using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FasterMate.Infrastructure.Data
{
    public class Comment
    {
        [Key]
        [MaxLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(100)]
        public string Text { get; set; }

        [Required]
        [ForeignKey(nameof(Post))]
        [MaxLength(36)]
        public string ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        [Required]
        [ForeignKey(nameof(Post))]
        [MaxLength(36)]
        public string PostId { get; set; }
        public virtual Post Post { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        //public virtual ICollection<CommentLike> CommentsLikes { get; set; } = new HashSet<CommentLike>();
    }
}
