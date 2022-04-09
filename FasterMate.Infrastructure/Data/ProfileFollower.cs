namespace FasterMate.Infrastructure.Data
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ProfileFollower
    {
        [Required]
        [ForeignKey(nameof(Profile))]
        [MaxLength(36)]
        public string ProfileId { get; set; }

        public virtual Profile Profile { get; set; }

        [Required]
        [ForeignKey(nameof(Follower))]
        [MaxLength(36)]
        public string FollowerId { get; set; }

        public virtual Profile Follower { get; set; }
    }
}
