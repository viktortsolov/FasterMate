namespace FasterMate.Infrastructure.Data
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PostLike
    {
        [Required]
        [ForeignKey(nameof(Post))]
        public string PostId { get; set; }

        public virtual Post Post { get; set; }

        [Required]
        [ForeignKey(nameof(Profile))]
        public string ProfileId { get; set; }

        public virtual Profile Profile { get; set; }
    }
}
