namespace FasterMate.Core.Contracts
{
    using FasterMate.ViewModels.Offer;

    public interface IOfferService
    {
        Task<bool> CreateAsync(CreateOfferViewModel input);

        IEnumerable<RenderOfferViewModel> RenderOffers();
    }
}
