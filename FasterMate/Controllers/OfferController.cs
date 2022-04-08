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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = profileService.GetId(userId);

            var viewModel = new CreateOfferViewModel() { ProfileId = profileId };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddAnOffer(CreateOfferViewModel input)
        {
            var offer = await offerService.CreateAsync(input);
            if (offer == false)
            {
                ViewData[MessageConstant.ErrorMessage] = "ArrivalTime cannot be before the Departure Time!";
                return View(input);
            }

            if (!ModelState.IsValid)
            {
                return View(input);
            }

            return RedirectToAction(nameof(BookAFlight));
        }
    }
}
