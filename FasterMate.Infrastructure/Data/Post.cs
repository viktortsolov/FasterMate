namespace FasterMate.Infrastructure.Data
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Post
    {
        [Key]
        [MaxLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(256)]
        public string Text { get; set; }

        [Required]
        [MaxLength(128)]
        public string Location { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [Required]
        [ForeignKey(nameof(Image))]
        [MaxLength(36)]
        public string ImageId { get; set; }

        public virtual Image Image { get; set; }

        [Required]
        [ForeignKey(nameof(Profile))]
        [MaxLength(36)]
        public string ProfileId { get; set; }

        public virtual Profile Profile { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public virtual ICollection<PostLike> PostLikes { get; set; } = new HashSet<PostLike>();
    }
}
