namespace FasterMate.Controllers
{
    using System.Security.Claims;

    using FasterMate.Core.Constants;
    using FasterMate.Core.Contracts;
    using FasterMate.ViewModels.Offer;

    using Microsoft.AspNetCore.Mvc;

    public class OfferController : BaseController
    {
        private readonly IProfileService profileService;
        private readonly IOfferService offerService;

        public OfferController(
            IProfileService _profileService,
            IOfferService _offerService)
        {
            profileService = _profileService;
            offerService = _offerService;
        }

        public IActionResult BookAFlight()
        {
            var offers = offerService.RenderOffers();

            return View(offers);
        }

        public IActionResult MyFlights()
        {
            return View();
        }

        public IActionResult AddAnOffer()
        {
            var viewModel = new CreateOfferViewModel();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddAnOffer(CreateOfferViewModel input)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = profileService.GetId(userId);
            
            var offer = await offerService.CreateAsync(input, profileId);
            if (offer == false)
            {
                ViewData[MessageConstant.ErrorMessage] = "Arrival Time cannot be before the Departure Time!";
                return View(input);
            }

            if (!ModelState.IsValid)
            {
                return View(input);
            }

            return RedirectToAction(nameof(BookAFlight));
        }

        public async Task<IActionResult> BookTicket(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = profileService.GetId(userId);

            await offerService.CreateProfileOfferAsync(id, profileId);
            
            return RedirectToAction(nameof(BookAFlight));
        }
    }
}
