namespace FasterMate.Areas.Administrator.Controllers
{
    using FasterMate.Core.Contracts;
    using Microsoft.AspNetCore.Mvc;

    public class OfferController : BaseController
    {
        private readonly IOfferService offerService;

        public OfferController(IOfferService _offerService)
        {
            offerService = _offerService;
        }

        public IActionResult ManageOffers()
        {
            var offers = offerService.GetAllOffersForAdministratior();

            return View(offers);
        }

        public async Task<IActionResult> DeleteOffer(string id)
        {
            await offerService.DeleteAsync(id);

            return RedirectToAction(nameof(ManageOffers));
        }
    }
}
