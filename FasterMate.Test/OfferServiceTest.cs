namespace FasterMate.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FasterMate.Core.Contracts;
    using FasterMate.Core.Services;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;
    using FasterMate.Infrastructure.Data.Enums;
    using FasterMate.ViewModels.Home;
    using FasterMate.ViewModels.Offer;
    using FasterMate.ViewModels.Profile;

    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;

    public class OfferServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;

        [SetUp]
        public async Task Setup()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollecton = new ServiceCollection();

            serviceProvider = serviceCollecton
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton(typeof(IRepository<>), typeof(Repository<>))
                .AddSingleton<IOfferService, OfferService>()
                .BuildServiceProvider();

            await SeedDb();
        }

        [Test]
        public async Task CreateSuccessfully()
        {
            var offerService = serviceProvider.GetService<IOfferService>();

            var input = new CreateOfferViewModel()
            {
                ArrivalLocation = "Sofia",
                DepartureLocation = "Varna",
                ArrivalTime = (DateTime.Now).ToString(),
                DepartureTime = (DateTime.UtcNow).ToString(),
                PriceOfTicket = 12
            };

            var result = await offerService.CreateAsync(input, "c996abfe-1850-48dd-bfcd-b61f18ec3358");

            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task CreateUnsuccessfully()
        {
            var offerService = serviceProvider.GetService<IOfferService>();

            var input = new CreateOfferViewModel()
            {
                ArrivalLocation = "Sofia",
                DepartureLocation = "Varna",
                ArrivalTime = (DateTime.UtcNow).ToString(),
                DepartureTime = (DateTime.Now).ToString(),
                PriceOfTicket = 12
            };

            var result = await offerService.CreateAsync(input, "c996abfe-1850-48dd-bfcd-b61f18ec3358");

            Assert.AreEqual(false, result);
        }

        [Test]
        public async Task CreateProfileOfferSuccessfully()
        {
            var profileOfferRepo = serviceProvider.GetService<IRepository<ProfileOffer>>();
            var offerService = serviceProvider.GetService<IOfferService>();

            await offerService.CreateProfileOfferAsync("test", "c996abfe-1850-48dd-bfcd-b61f18ec3358");

            Assert.AreEqual(1, profileOfferRepo.All().Count());
        }

        [Test]
        public async Task DeleteIfNotBookedSuccessfully()
        {
            var offerRepo = serviceProvider.GetService<IRepository<Offer>>();
            var offerService = serviceProvider.GetService<IOfferService>();

            await offerService.DeleteAsync("test");

            Assert.AreEqual(0, offerRepo.All().Count());
        }

        [Test]
        public async Task DeleteIfBookedSuccessfully()
        {
            var offerRepo = serviceProvider.GetService<IRepository<Offer>>();
            var offerService = serviceProvider.GetService<IOfferService>();

            await offerService.CreateProfileOfferAsync("test", "c996abfe-1850-48dd-bfcd-b61f18ec3358");
            await offerService.DeleteAsync("test");

            Assert.AreEqual(0, offerRepo.All().Count());
        }

        [Test]
        public async Task GetAllOffersSuccessfully()
        {
            var offerService = serviceProvider.GetService<IOfferService>();

            var expected = new List<RenderOfferViewModel>();
            expected.Add(new RenderOfferViewModel()
            {
                Id = "test",
                ArrivalLocation = "Sofia",
                DepartureLocation = "Varna",
                ArrivalTime = DateTime.Now.ToString("dd.MM.yyyy a\\t HH:mm"),
                DepartureTime = DateTime.UtcNow.ToString("dd.MM.yyyy a\\t HH:mm"),
                PriceOfTicket = 12.ToString("f2"),
                IsBooked = false,
                Name = "test test"
            });

            var input = new CreateOfferViewModel()
            {
                ArrivalLocation = "Sofia",
                DepartureLocation = "Varna",
                ArrivalTime = (DateTime.Now.AddMonths(2)).ToString(),
                DepartureTime = (DateTime.UtcNow.AddMonths(2)).ToString(),
                PriceOfTicket = 12
            };

            var result = await offerService.CreateAsync(input, "c996abfe-1850-48dd-bfcd-b61f18ec3358");

            var actual = offerService.GetAllOffers();

            Assert.AreEqual(expected.FirstOrDefault().ArrivalLocation, actual.FirstOrDefault().ArrivalLocation);
        }

        [Test]
        public void GetAllOffersForAdminSuccessfully()
        {
            var offerService = serviceProvider.GetService<IOfferService>();

            var expected = new List<RenderAdministratorOfferViewModel>();
            expected.Add(new RenderAdministratorOfferViewModel()
            {
                Id = "test",
                ArrivalLocation = "Sofia",
                DepartureLocation = "Varna",
                ArrivalTime = DateTime.Now.ToString("dd.MM.yyyy a\\t HH:mm"),
                DepartureTime = DateTime.UtcNow.ToString("dd.MM.yyyy a\\t HH:mm"),
                PriceOfTicket = 12.ToString("f2"),
                Name = "test test",
                ProfileId = "test"
            });

            var actual = offerService.GetAllOffersForAdministratior();

            Assert.AreEqual(expected.FirstOrDefault().ProfileId, actual.FirstOrDefault().ProfileId);
        }

        [Test]
        public async Task GetBookedOffersOfProfileSuccessfully()
        {
            var offerService = serviceProvider.GetService<IOfferService>();

            var expected = new List<MyOffersViewModel>();
            expected.Add(new MyOffersViewModel()
            {
                Id = "test",
                ArrivalLocation = "Sofia",
                DepartureLocation = "Varna",
                ArrivalTime = DateTime.Now.ToString("dd.MM.yyyy a\\t HH:mm"),
                DepartureTime = DateTime.UtcNow.ToString("dd.MM.yyyy a\\t HH:mm"),
                PriceOfTicket = 12.ToString("f2"),
                Name = "test test"
            });

            await offerService.CreateProfileOfferAsync("test", "c996abfe-1850-48dd-bfcd-b61f18ec3358");
            var actual = offerService.BookedOffersOfProfile("c996abfe-1850-48dd-bfcd-b61f18ec3358");

            Assert.AreEqual(expected.FirstOrDefault().Name, actual.FirstOrDefault().Name);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private async Task SeedDb()
        {
            var countryRepo = serviceProvider.GetService<IRepository<Country>>();
            var imgRepo = serviceProvider.GetService<IRepository<Image>>();
            var profileRepo = serviceProvider.GetService<IRepository<Profile>>();
            var offerRepo = serviceProvider.GetService<IRepository<Offer>>();

            await countryRepo.AddAsync(new Country() { Id = "test", Name = "test" });
            await countryRepo.SaveChangesAsync();

            await imgRepo.AddAsync(new Image() { Id = "test", Extension = "test" });
            await imgRepo.SaveChangesAsync();

            await profileRepo.AddAsync(new Profile()
            {
                Id = "c996abfe-1850-48dd-bfcd-b61f18ec3358",
                FirstName = "test",
                LastName = "test",
                CountryId = countryRepo.AllAsNoTracking().Select(x => x.Id).FirstOrDefault(),
                BirthDate = DateTime.UtcNow,
                Gender = Gender.Male,
                User = new ApplicationUser()
                {
                    Id = "310b8a7e-734d-44d6-b3f3-efa6e8f6259d",
                    UserName = "test1",
                    Email = "email@gmail.com"
                },
                ImageId = imgRepo.AllAsNoTracking().Select(x => x.Id).FirstOrDefault(),
            });

            await profileRepo.AddAsync(new Profile()
            {
                Id = "test",
                FirstName = "test",
                LastName = "test",
                CountryId = countryRepo.AllAsNoTracking().Select(x => x.Id).FirstOrDefault(),
                BirthDate = DateTime.UtcNow,
                Gender = Gender.Male,
                User = new ApplicationUser()
                {
                    Id = "test",
                    Email = "abv1234@gmail.com",
                    UserName = "test"
                },
                ImageId = imgRepo.AllAsNoTracking().Select(x => x.Id).FirstOrDefault(),
            });
            await profileRepo.SaveChangesAsync();

            await offerRepo.AddAsync(new Offer()
            {
                Id = "test",
                ArrivalLocation = "Sofia",
                DepartureLocation = "Varna",
                ArrivalTime = DateTime.Now.AddDays(1),
                DepartureTime = DateTime.UtcNow.AddDays(1),
                PriceOfTicket = 12,
                ProfileId = "test"
            });
            await offerRepo.SaveChangesAsync();
        }
    }
}
