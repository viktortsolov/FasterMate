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
    using FasterMate.ViewModels.Profile;

    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;

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
            var imageRepo = serviceProvider.GetService<IRepository<Image>>();

            await SeedDb(profileRepo, countryRepo, followerRepo, imageRepo);
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

        [Test]
        public void GetEditProfileSuccessfully()
        {
            var profileService = serviceProvider.GetService<IProfileService>();
            var profileRepo = serviceProvider.GetService<IRepository<Profile>>();

            var profile = profileRepo.All().FirstOrDefault();
            var actual = profileService.GetEditViewModel(profile);

            var expected = new EditProfileViewModel()
            {
                Id = profile.Id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Bio = profile.Bio
            };

            Assert.AreEqual(expected.FirstName, actual.FirstName);
        }

        [Test]
        public void GetRenderProfileSuccessfully()
        {
            var profileService = serviceProvider.GetService<IProfileService>();
            var profileRepo = serviceProvider.GetService<IRepository<Profile>>();
            var countryRepo = serviceProvider.GetService<IRepository<Country>>();
            var followerRepo = serviceProvider.GetService<IRepository<ProfileFollower>>();

            var profile = profileRepo.All().FirstOrDefault();
            var actual = profileService.RenderProfile(profile.Id);

            var expected = new RenderProfileViewModel()
            {
                Id = "c996abfe-1850-48dd-bfcd-b61f18ec3358",
                FirstName = "test",
                LastName = "test",
                IsFollowing = followerRepo.All().Any(x => x.ProfileId == "c996abfe-1850-48dd-bfcd-b61f18ec3358"),
                Gender = Gender.Male.ToString(),
                Birthdate = DateTime.UtcNow,
                Bio = "",
                Country = countryRepo.AllAsNoTracking().Select(x => x.Id).FirstOrDefault(),
                FollowingCount = 0,
                FollowersCount = 0,
                ImagePath = "test1234-test-test-test-test1234test.test"
            };

            Assert.AreEqual(expected.ImagePath, actual.ImagePath);
        }

        [Test]
        public async Task FollowUserSuccessfully()
        {
            var profileService = serviceProvider.GetService<IProfileService>();
            var profileRepo = serviceProvider.GetService<IRepository<Profile>>();
            var countryRepo = serviceProvider.GetService<IRepository<Country>>();
            var imageRepo = serviceProvider.GetService<IRepository<Image>>();
            var followerRepo = serviceProvider.GetService<IRepository<ProfileFollower>>();

            await profileRepo.AddAsync(new Profile()
            {
                Id = "test1234-test-test-test-test1234test",
                FirstName = "test",
                LastName = "test",
                CountryId = countryRepo.AllAsNoTracking().Select(x => x.Id).FirstOrDefault(),
                BirthDate = DateTime.UtcNow,
                Gender = Gender.Male,
                User = new ApplicationUser()
                {
                    Id = "test1234-test-test-test-test1234test"
                },
                ImageId = imageRepo.AllAsNoTracking().Select(x => x.Id).FirstOrDefault(),
            });

            await profileRepo.SaveChangesAsync();

            var current = profileRepo.All().FirstOrDefault().Id;
            var asking = profileRepo.All().Where(x => x.Id == "test1234-test-test-test-test1234test").FirstOrDefault().Id;

            await profileService.FollowProfileAsync(current, asking);

            var expected = "test1234-test-test-test-test1234test";
            var actual = followerRepo.All().FirstOrDefault().FollowerId;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task UnfollowUserSuccessfully()
        {
            var profileService = serviceProvider.GetService<IProfileService>();
            var profileRepo = serviceProvider.GetService<IRepository<Profile>>();
            var countryRepo = serviceProvider.GetService<IRepository<Country>>();
            var imageRepo = serviceProvider.GetService<IRepository<Image>>();
            var followerRepo = serviceProvider.GetService<IRepository<ProfileFollower>>();

            await profileRepo.AddAsync(new Profile()
            {
                Id = "test1234-test-test-test-test1234test",
                FirstName = "test",
                LastName = "test",
                CountryId = countryRepo.AllAsNoTracking().Select(x => x.Id).FirstOrDefault(),
                BirthDate = DateTime.UtcNow,
                Gender = Gender.Male,
                User = new ApplicationUser()
                {
                    Id = "test1234-test-test-test-test1234test"
                },
                ImageId = imageRepo.AllAsNoTracking().Select(x => x.Id).FirstOrDefault(),
            });

            await profileRepo.SaveChangesAsync();

            var current = profileRepo.All().FirstOrDefault().Id;
            var asking = profileRepo.All().Where(x => x.Id == "test1234-test-test-test-test1234test").FirstOrDefault().Id;

            await profileService.FollowProfileAsync(current, asking);
            await profileService.FollowProfileAsync(current, asking);

            var expected = "test1234-test-test-test-test1234test";
            var actual = followerRepo.All().FirstOrDefault();

            Assert.AreNotEqual(expected, actual);
        }

        [Test]
        public void SearchProfilesSuccessfully()
        {
            var profileService = serviceProvider.GetService<IProfileService>();
            var imageRepo = serviceProvider.GetService<IRepository<Image>>();

            var img = imageRepo.AllAsNoTracking().FirstOrDefault();
            var expected = new List<ProfileSearchViewModel>();
            expected.Add(new ProfileSearchViewModel()
            {
                Id = "c996abfe-1850-48dd-bfcd-b61f18ec3358",
                FirstName = "test",
                LastName = "test",
                Username = "test123@gmail.com",
                ImagePath = $"{img.Id}.{img.Extension}",
                Followers = 0,
                Following = 0
            });

            string[] tokens = new string[1];
            tokens[0] = "test";

            var actual = profileService.SearchProfiles(tokens);

            Assert.AreEqual(expected.FirstOrDefault().FirstName, actual.FirstOrDefault().FirstName);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private static async Task SeedDb(IRepository<Profile> profileRepo, IRepository<Country> countryRepo, IRepository<ProfileFollower> followerRepo, IRepository<Image> imageRepo)
        {
            await countryRepo.AddAsync(new Country() { Id = "test1234-test-test-test-test1234test", Name = "test" });
            await imageRepo.AddAsync(new Image() { Id = "test1234-test-test-test-test1234test", Extension = "test" });
            await countryRepo.SaveChangesAsync();

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
                    Id = "310b8a7e-734d-44d6-b3f3-efa6e8f6259d"
                },
                ImageId = imageRepo.AllAsNoTracking().Select(x => x.Id).FirstOrDefault(),
            });

            await profileRepo.SaveChangesAsync();
        }
    }
}
