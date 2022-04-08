﻿namespace FasterMate.Core.Services
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

        public OfferService(
            IRepository<Offer> _offerRepo,
            IRepository<ProfileOffer> _profileOfferRepo)
        {
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
            var profileOffer = new ProfileOffer()
            {
                OfferId = id,
                ProfileId = profileId
            };

            var offer = offerRepo.AllAsNoTracking().FirstOrDefault(x => x.Id == id);
            offer.IsBooked = true;
            offerRepo.Update(offer);

            await profileOfferRepo.AddAsync(profileOffer);
            await profileOfferRepo.SaveChangesAsync();
        }

        public IEnumerable<RenderOfferViewModel> RenderOffers()
            => offerRepo
                    .AllAsNoTracking()
                    .Include(x => x.Owner)
                    .OrderByDescending(x => x.DepartureTime)
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
    }
}
