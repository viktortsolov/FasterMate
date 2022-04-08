namespace FasterMate.Core.Contracts
{
    using FasterMate.ViewModels.Offer;

    public interface IOfferService
    {
        Task<bool> CreateAsync(CreateOfferViewModel input, string profileId);

        IEnumerable<RenderOfferViewModel> RenderOffers();

        Task CreateProfileOfferAsync(string id, string profileId);
    }
}
