namespace FasterMate.Test
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using FasterMate.Core.Contracts;
    using FasterMate.Core.Services;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;
    using FasterMate.Infrastructure.Data.Enums;
    using FasterMate.ViewModels.Group;
    using FasterMate.ViewModels.User;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;

    public class UserServiceTest
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
                .AddSingleton<IUserService, UserService>()
                .BuildServiceProvider();

            await SeedDb();
        }

        [Test]
        public async Task GetOnlyAppUserSuccessfully()
        {
            var userService = serviceProvider.GetService<IUserService>();

            var expected = "test";
            var actual = await userService.GetOnlyUserById("test1234-test-test-test-test1234test");

            Assert.AreEqual(expected, actual.UserName);
        }

        [Test]
        public async Task GetAppUserPlusProfileSuccessfully()
        {
            var userService = serviceProvider.GetService<IUserService>();

            var expected = "test";
            var actual = await userService.GetUserById("test1234-test-test-test-test1234test");

            Assert.AreEqual(expected, actual.Profile.FirstName);
        }

        [Test]
        public async Task GetUserForEditSuccessfully()
        {
            var userService = serviceProvider.GetService<IUserService>();

            var expected = new UserEditViewModel()
            {
                Id = "310b8a7e-734d-44d6-b3f3-efa6e8f6259d",
                FirstName = "test",
                LastName = "test"
            };

            var actual = await userService.GetUserForEdit("310b8a7e-734d-44d6-b3f3-efa6e8f6259d");

            Assert.AreEqual(actual.Id, expected.Id);
            Assert.AreEqual(actual.FirstName, expected.FirstName);
            Assert.AreEqual(actual.LastName, expected.LastName);
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
            var groupRepo = serviceProvider.GetService<IRepository<Group>>();
            var profileFollowerRepo = serviceProvider.GetService<IRepository<ProfileFollower>>();
            var msgRepo = serviceProvider.GetService<IRepository<Message>>();

            await countryRepo.AddAsync(new Country() { Id = "test1234-test-test-test-test1234test", Name = "test" });
            await countryRepo.SaveChangesAsync();

            await imgRepo.AddAsync(new Image() { Id = "test1234-test-test-test-test1234test", Extension = "test" });
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
                    Id = "310b8a7e-734d-44d6-b3f3-efa6e8f6259d"
                },
                ImageId = imgRepo.AllAsNoTracking().Select(x => x.Id).FirstOrDefault(),
            });

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
                    Id = "test1234-test-test-test-test1234test",
                    UserName = "test"
                },
                ImageId = imgRepo.AllAsNoTracking().Select(x => x.Id).FirstOrDefault(),
            });
            await profileRepo.SaveChangesAsync();

            await groupRepo.AddAsync(new Group()
            {
                Id = "test1234-test-test-test-test1234test",
                Name = "Test Group Name",
                ImageId = "test1234-test-test-test-test1234test",
                ProfileId = "test1234-test-test-test-test1234test"
            });
            await groupRepo.SaveChangesAsync();

            await msgRepo.AddAsync(new Message()
            {
                Id = "test1234-test-test-test-test1234test",
                Text = "some test text",
                GroupId = "test1234-test-test-test-test1234test",
                ProfileId = "test1234-test-test-test-test1234test",
                CreatedOn = DateTime.UtcNow
            });
            await msgRepo.SaveChangesAsync();

            await profileFollowerRepo.AddAsync(new ProfileFollower()
            {
                ProfileId = "test1234-test-test-test-test1234test",
                FollowerId = "c996abfe-1850-48dd-bfcd-b61f18ec3358"
            });
            await profileFollowerRepo.SaveChangesAsync();
        }
    }
}
