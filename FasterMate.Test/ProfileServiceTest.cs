namespace FasterMate.Test
{
    using FasterMate.Core.Contracts;
    using FasterMate.Core.Services;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;
    using FasterMate.Infrastructure.Data.Enums;
    using FasterMate.ViewModels.Profile;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProfileServiceTest
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
                .AddSingleton<IProfileService, ProfileService>()
                .AddSingleton<IImageService, ImageService>()
                .AddSingleton<IPostService, PostService>()
                .AddSingleton<ICommentService, CommentService>()
                .BuildServiceProvider();

            var profileRepo = serviceProvider.GetService<IRepository<Profile>>();
            var countryRepo = serviceProvider.GetService<IRepository<Country>>();
            var followerRepo = serviceProvider.GetService<IRepository<ProfileFollower>>();

            await SeedDb(profileRepo, countryRepo, followerRepo);
        }

        [Test]
        public void CreateMustThrowAEWhenCountryIsNotValid()
        {
            var register = new RegisterViewModel()
            {
                Username = "test",
                Email = "test123@gmail.com",
                FirstName = "test",
                LastName = "test",
                BirthDate = (DateTime.UtcNow).ToString(),
                Gender = (Gender.Male).ToString(),
                CountryId = "test4321-test-test-test-test1234test",
                Password = "test1234test1234test1234",
                ConfirmPassword = "test1234test1234test1234"
            };

            var profileService = serviceProvider.GetService<IProfileService>();

            Assert.CatchAsync<ArgumentException>(async () => await profileService.CreateAsync(register), "The selected country must be valid!");
        }

        [Test]
        public void CreateMustThrowAEWhenGenderIsNotValid()
        {
            var register = new RegisterViewModel()
            {
                Username = "test",
                Email = "test123@gmail.com",
                FirstName = "test",
                LastName = "test",
                BirthDate = (DateTime.UtcNow).ToString(),
                Gender = "Gotin",
                CountryId = "test1234-test-test-test-test1234test",
                Password = "test1234test1234test1234",
                ConfirmPassword = "test1234test1234test1234"
            };
            var profileService = serviceProvider.GetService<IProfileService>();

            Assert.CatchAsync<ArgumentException>(async () => await profileService.CreateAsync(register), "The selected gender must be valid!");
        }

        [Test]
        public void CreateMustThrowAEWhenDateIsNotValid()
        {
            var register = new RegisterViewModel()
            {
                Username = "test",
                Email = "test123@gmail.com",
                FirstName = "test",
                LastName = "test",
                BirthDate = "01/01/1899 00:00",
                Gender = Gender.Male.ToString(),
                CountryId = "test1234-test-test-test-test1234test",
                Password = "test1234test1234test1234",
                ConfirmPassword = "test1234test1234test1234"
            };
            var profileService = serviceProvider.GetService<IProfileService>();

            Assert.CatchAsync<ArgumentException>(async () => await profileService.CreateAsync(register), "Birth Date must be after the year of 1900!");
        }

        [Test]
        public void CreateMustBeSuccessfully()
        {
            var register = new RegisterViewModel()
            {
                Username = "test",
                Email = "test123@gmail.com",
                FirstName = "test",
                LastName = "test",
                BirthDate = DateTime.UtcNow.ToString(),
                Gender = Gender.Male.ToString(),
                CountryId = "test1234-test-test-test-test1234test",
                Password = "test1234test1234test1234",
                ConfirmPassword = "test1234test1234test1234"
            };
            var profileService = serviceProvider.GetService<IProfileService>();

            Assert.DoesNotThrowAsync(async () => await profileService.CreateAsync(register));
        }

        [Test]
        public void GetUserIdSuccessfully()
        {
            var profileService = serviceProvider.GetService<IProfileService>();

            var expected = "c996abfe-1850-48dd-bfcd-b61f18ec3358";
            var actual = profileService.GetId("310b8a7e-734d-44d6-b3f3-efa6e8f6259d");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetUserByUserIdSuccessfully()
        {
            var profileService = serviceProvider.GetService<IProfileService>();

            var actual = profileService.GetByUserId("310b8a7e-734d-44d6-b3f3-efa6e8f6259d").Id;
            var expected = new Profile()
            {
                Id = "c996abfe-1850-48dd-bfcd-b61f18ec3358",
                FirstName = "test",
                LastName = "test",
                CountryId = "test1234-test-test-test-test1234test",
                BirthDate = DateTime.UtcNow,
                User = new ApplicationUser()
                {
                    Id = "310b8a7e-734d-44d6-b3f3-efa6e8f6259d"
                }
            }.Id;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetUserByProfileIdSuccessfully()
        {
            var profileService = serviceProvider.GetService<IProfileService>();

            var actual = profileService.GetById("c996abfe-1850-48dd-bfcd-b61f18ec3358").Id;
            var expected = new Profile()
            {
                Id = "c996abfe-1850-48dd-bfcd-b61f18ec3358",
                FirstName = "test",
                LastName = "test",
                CountryId = "test1234-test-test-test-test1234test",
                BirthDate = DateTime.UtcNow,
                User = new ApplicationUser()
                {
                    Id = "310b8a7e-734d-44d6-b3f3-efa6e8f6259d"
                }
            }.Id;

            Assert.AreEqual(expected, actual);
        }


        [Test]
        public async Task UpdateAsyncDoesntWork()
        {
            var profileService = serviceProvider.GetService<IProfileService>();
            var profileRepo = serviceProvider.GetService<IRepository<Profile>>();

            var input = new EditProfileViewModel();
            await profileService.UpdateAsync("K996abfe-1850-48dd-bfcd-b61f18ec3358", input, "test");

            Assert.AreEqual("c996abfe-1850-48dd-bfcd-b61f18ec3358", profileRepo.AllAsNoTracking().FirstOrDefault().Id);
        }

        [Test]
        public async Task UpdateAsyncSuccessfully()
        {
            var profileService = serviceProvider.GetService<IProfileService>();
            var profileRepo = serviceProvider.GetService<IRepository<Profile>>();
            
            var input = new EditProfileViewModel()
            {
                FirstName = "Different Name",
                LastName = "Last Name is different too",
                Image = null
            };

            await profileService.UpdateAsync("310b8a7e-734d-44d6-b3f3-efa6e8f6259d", input, "somePath");

            Assert.AreEqual("Different Name", profileRepo.AllAsNoTracking().FirstOrDefault().FirstName);
        }


        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private static async Task SeedDb(IRepository<Profile> profileRepo, IRepository<Country> countryRepo, IRepository<ProfileFollower> followerRepo)
        {
            await countryRepo.AddAsync(new Country() { Id = "test1234-test-test-test-test1234test", Name = "test" });
            await countryRepo.SaveChangesAsync();

            await profileRepo.AddAsync(new Profile()
            {
                Id = "c996abfe-1850-48dd-bfcd-b61f18ec3358",
                FirstName = "test",
                LastName = "test",
                CountryId = countryRepo.AllAsNoTracking().Select(x => x.Id).FirstOrDefault(),
                BirthDate = DateTime.UtcNow,
                User = new ApplicationUser()
                {
                    Id = "310b8a7e-734d-44d6-b3f3-efa6e8f6259d"
                }
            });
            await profileRepo.SaveChangesAsync();
        }
    }
}
