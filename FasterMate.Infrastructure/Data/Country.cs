namespace FasterMate.Infrastructure.Data
{
    using System.ComponentModel.DataAnnotations;

    public class Country
    {
        public Country()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
