namespace FasterMate.Core.Contracts
{
    using FasterMate.ViewModels.Offer;

    public interface IOfferService
    {
        Task<bool> CreateAsync(CreateOfferViewModel input, string profileId);

        IEnumerable<RenderOfferViewModel> GetAllOffers();

        Task CreateProfileOfferAsync(string id, string profileId);

        IEnumerable<RenderAdministratorOfferViewModel> GetAllOffersForAdministratior();

        Task DeleteAsync(string id);
    }
}
