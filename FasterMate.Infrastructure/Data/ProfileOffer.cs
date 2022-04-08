namespace FasterMate.Infrastructure.Data
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class ProfileOffer
    {
        [ForeignKey(nameof(Profile))]
        public string ProfileId { get; set; }
        public Profile Profile { get; set; }


        [ForeignKey(nameof(Offer))]
        public string OfferId { get; set; }
        public Offer Offer { get; set; }
    }
}
