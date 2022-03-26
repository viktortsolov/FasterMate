namespace FasterMate.Infrastructure.Data
{
    using System.ComponentModel.DataAnnotations;

    public class Country
    {
        [Key]
        [MaxLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(64)]
        public string Name { get; set; }
    }
}
