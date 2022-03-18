namespace FasterMate.Infrastructure.Data
{
    using System.ComponentModel.DataAnnotations;

    public class Image
    {
        public Image()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Extension { get; set; }

        public virtual ICollection<Profile> Profiles { get; set; }
    }
}
