namespace FasterMate.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FasterMate.Core.Contracts;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;
    using FasterMate.ViewModels.Offer;

    public class OfferService : IOfferService
    {
        private readonly IRepository<Offer> offerRepo;

        public OfferService(IRepository<Offer> _offerRepo)
        {
            offerRepo = _offerRepo;
        }


        public async Task<bool> CreateAsync(CreateOfferViewModel input)
        {
            var offer = new Offer()
            {
                ArrivalLocation = input.ArrivalLocation,
                DepartureLocation = input.DepartureLocation,
                ArrivalTime = input.ArrivalTime,
                DepartureTime = input.DepartureTime,
                PriceOfTicket = input.PriceOfTicket,
                ProfileId = input.ProfileId
            };

            if (offer.ArrivalTime < offer.DepartureTime)
            {
                return false;
            }

            await offerRepo.AddAsync(offer);
            await offerRepo.SaveChangesAsync();
            return true;
        }

        public IEnumerable<RenderOfferViewModel> RenderOffers()
            => offerRepo
                    .AllAsNoTracking()
                    .OrderByDescending(x => x.DepartureTime)
                    .Select(x => new RenderOfferViewModel()
                    {
                        Id = x.Id,
                        ArrivalLocation = x.ArrivalLocation,
                        DepartureLocation = x.DepartureLocation,
                        ArrivalTime = x.ArrivalTime,
                        DepartureTime= x.DepartureTime,
                        PriceOfTicket = x.PriceOfTicket,
                        ProfileId = x.ProfileId
                    })
                    .ToList();
    }
}
