namespace FasterMate.ViewModels.Offer
{
    public class RenderOfferViewModel
    {
        public string Id { get; set; }

        public string DepartureLocation { get; set; }

        public string ArrivalLocation { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public decimal PriceOfTicket { get; set; }

        public string ProfileId { get; set; }
    }
}
