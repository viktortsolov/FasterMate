namespace FasterMate.Infrastructure.Data
{
    using System.ComponentModel.DataAnnotations;

    public class Image
    {
        [Key]
        [MaxLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Extension { get; set; }

        public virtual ICollection<Profile> Profiles { get; set; }
    }
}
