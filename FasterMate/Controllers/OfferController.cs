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
            var offers = offerService.GetAllOffers();

            return View(offers);
        }

        public IActionResult MyFlights()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profileId = profileService.GetId(userId);

            var viewModel = offerService.BookedOffersOfProfile(profileId);

            return View(viewModel);
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

            if (!ModelState.IsValid)
            {
                return View(input);
            }

            if (DateTime.Parse(input.DepartureTime) < DateTime.Now)
            {
                ViewData[MessageConstant.ErrorMessage] = "Your departure time is invalid!";
                return View(input);
            }

            if (DateTime.Parse(input.ArrivalTime) < DateTime.Now)
            {
                ViewData[MessageConstant.ErrorMessage] = "Your arrival time is invalid!";
                return View(input);
            }

            var offer = await offerService.CreateAsync(input, profileId);
            if (offer == false)
            {
                ViewData[MessageConstant.ErrorMessage] = "Arrival Time cannot be before the Departure Time!";
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
