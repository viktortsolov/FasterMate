namespace FasterMate.ViewModels.Offer
{
    using System.ComponentModel.DataAnnotations;

    public class CreateOfferViewModel
    {
        [Required]
        [StringLength(128, ErrorMessage = "Departure Location must be at most {1} characters long.")]
        [Display(Name = "Departure Location")]
        public string DepartureLocation { get; set; }

        [Required]
        [StringLength(128, ErrorMessage = "Arrival Location must be at most {1} characters long.")]
        [Display(Name = "Arrival Location")]
        public string ArrivalLocation { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Departure Time")]
        public string DepartureTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Arrival Time")]
        public string ArrivalTime { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Price of the Ticket")]
        [Range(1, 1000, ErrorMessage = "Price of the Ticket must be between 1$ and 1000$.")]
        public decimal PriceOfTicket { get; set; }
    }
}
