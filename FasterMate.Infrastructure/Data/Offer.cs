namespace FasterMate.Infrastructure.Data
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Offer
    {
        [Key]
        [MaxLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(128)]
        public string DepartureLocation { get; set; }

        [Required]
        [MaxLength(128)]
        public string ArrivalLocation { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal PriceOfTicket { get; set; }

        public bool IsBooked { get; set; } = false;

        public string ProfileId { get; set; }

        [ForeignKey(nameof(ProfileId))]
        public Profile Owner { get; set; }
    }
}