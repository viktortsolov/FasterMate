namespace FasterMate.Infrastructure.Data
{
    using FasterMate.Infrastructure.Data.Enums;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Profile
    {
        [Key]
        [MaxLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(64)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(64)]
        public string LastName { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime BirthDate { get; set; }

        public string? Bio { get; set; }


        [ForeignKey(nameof(Image))]
        public string? ImageId { get; set; }
        public virtual Image? Image { get; set; }


        [Required]
        [ForeignKey(nameof(Country))]
        public string? CountryId { get; set; }
        public virtual Country? Country { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<ProfileFollower> Followers { get; set; } = new HashSet<ProfileFollower>();
        public virtual ICollection<ProfileFollower> Following { get; set; } = new HashSet<ProfileFollower>();

        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    }
}