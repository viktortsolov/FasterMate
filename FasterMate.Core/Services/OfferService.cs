namespace FasterMate.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FasterMate.Core.Contracts;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;
    using FasterMate.ViewModels.Offer;
    using Microsoft.EntityFrameworkCore;

    public class OfferService : IOfferService
    {
        private readonly IRepository<Offer> offerRepo;
        private readonly IRepository<ProfileOffer> profileOfferRepo;
        private readonly IRepository<Profile> profileRepo;

        public OfferService(
            IRepository<Offer> _offerRepo,
            IRepository<Profile> _profileRepo,
            IRepository<ProfileOffer> _profileOfferRepo)
        {
            profileRepo = _profileRepo;
            offerRepo = _offerRepo;
            profileOfferRepo = _profileOfferRepo;
        }


        public async Task<bool> CreateAsync(CreateOfferViewModel input, string profileId)
        {
            var offer = new Offer()
            {
                ArrivalLocation = input.ArrivalLocation,
                DepartureLocation = input.DepartureLocation,
                ArrivalTime = DateTime.Parse(input.ArrivalTime),
                DepartureTime = DateTime.Parse(input.DepartureTime),
                PriceOfTicket = input.PriceOfTicket,
                ProfileId = profileId
            };

            if (offer.ArrivalTime < offer.DepartureTime)
            {
                return false;
            }

            await offerRepo.AddAsync(offer);
            await offerRepo.SaveChangesAsync();
            return true;
        }

        public async Task CreateProfileOfferAsync(string id, string profileId)
        {
            var profile = profileRepo.All().Where(x => x.Id == profileId).FirstOrDefault();
            var offer = offerRepo.All().Where(x => x.Id == id).FirstOrDefault();
            var profileOffer = new ProfileOffer()
            {
                Profile = profile,
                ProfileId = profileId,
                OfferId = id,
                Offer = offer
            };

            offer.IsBooked = true;
            offerRepo.Update(offer);

            await profileOfferRepo.AddAsync(profileOffer);
            await profileOfferRepo.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var offer = offerRepo
                .All()
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (offer != null)
            {
                if (offer.IsBooked)
                {
                    var profileOffer = profileOfferRepo
                        .All()
                        .Where(x => x.OfferId == id)
                        .FirstOrDefault();

                    profileOfferRepo.Delete(profileOffer);
                }

                offerRepo.Delete(offer);

                await offerRepo.SaveChangesAsync();
            }
        }

        public IEnumerable<RenderOfferViewModel> GetAllOffers()
            => offerRepo
                    .AllAsNoTracking()
                    .Include(x => x.Owner)
                    .OrderByDescending(x => x.DepartureTime)
                    .Where(x => x.DepartureTime >= DateTime.Now)
                    .Select(x => new RenderOfferViewModel()
                    {
                        Id = x.Id,
                        ArrivalLocation = x.ArrivalLocation,
                        DepartureLocation = x.DepartureLocation,
                        ArrivalTime = x.ArrivalTime.ToString("dd.MM.yyyy a\\t HH:mm"),
                        DepartureTime = x.DepartureTime.ToString("dd.MM.yyyy a\\t HH:mm"),
                        PriceOfTicket = x.PriceOfTicket.ToString("f2"),
                        IsBooked = x.IsBooked,
                        Name = $"{x.Owner.FirstName} {x.Owner.LastName}"
                    })
                    .ToList();

        public IEnumerable<RenderAdministratorOfferViewModel> GetAllOffersForAdministratior()
            => offerRepo
                    .AllAsNoTracking()
                    .Include(x => x.Owner)
                    .OrderByDescending(x => x.DepartureTime)
                    .Select(x => new RenderAdministratorOfferViewModel()
                    {
                        Id = x.Id,
                        ArrivalLocation = x.ArrivalLocation,
                        DepartureLocation = x.DepartureLocation,
                        ArrivalTime = x.ArrivalTime.ToString("dd.MM.yyyy a\\t HH:mm"),
                        DepartureTime = x.DepartureTime.ToString("dd.MM.yyyy a\\t HH:mm"),
                        PriceOfTicket = x.PriceOfTicket.ToString("f2"),
                        Name = $"{x.Owner.FirstName} {x.Owner.LastName}",
                        ProfileId = x.ProfileId
                    })
                    .ToList();

        public IEnumerable<MyOffersViewModel> BookedOffersOfProfile(string id)
            => profileOfferRepo
                    .AllAsNoTracking()
                    .Include(x => x.Offer)
                    .Include(x => x.Profile)
                    .OrderByDescending(x => x.Offer.DepartureTime)
                    .Where(x => x.Offer.DepartureTime >= DateTime.Now)
                    .Select(x => new MyOffersViewModel()
                    {
                        Id = x.Offer.Id,
                        ArrivalLocation = x.Offer.ArrivalLocation,
                        DepartureLocation = x.Offer.DepartureLocation,
                        ArrivalTime = x.Offer.ArrivalTime.ToString("dd.MM.yyyy a\\t HH:mm"),
                        DepartureTime = x.Offer.DepartureTime.ToString("dd.MM.yyyy a\\t HH:mm"),
                        PriceOfTicket = x.Offer.PriceOfTicket.ToString("f2"),
                        Name = $"{x.Profile.FirstName} {x.Profile.LastName}",
                        ProfileId = x.ProfileId
                    })
                    .ToList();
    }
}
