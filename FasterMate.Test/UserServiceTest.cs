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
    using FasterMate.ViewModels.User;

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
            var actual = await userService.GetOnlyUserByIdAsync("test1234-test-test-test-test1234test");

            Assert.AreEqual(expected, actual.UserName);
        }

        [Test]
        public async Task GetAppUserPlusProfileSuccessfully()
        {
            var userService = serviceProvider.GetService<IUserService>();

            var expected = "test";
            var actual = await userService.GetUserByIdAsync("test1234-test-test-test-test1234test");

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

            var actual = await userService.GetUserForEditAsync("310b8a7e-734d-44d6-b3f3-efa6e8f6259d");

            Assert.AreEqual(actual.Id, expected.Id);
            Assert.AreEqual(actual.FirstName, expected.FirstName);
            Assert.AreEqual(actual.LastName, expected.LastName);
        }

        [Test]
        public async Task GetAllUsersOfApplicationSuccessfully()
        {
            var userService = serviceProvider.GetService<IUserService>();

            var expected = new List<UserListViewModel>();
            expected.Add(new UserListViewModel()
            {
                Id = "310b8a7e-734d-44d6-b3f3-efa6e8f6259d",
                Email = "email@gmail.com",
                Username = "test1",
                Name = "test test"
            });
            expected.Add(new UserListViewModel()
            {
                Id = "test1234-test-test-test-test1234test",
                Email = "abv1234@gmail.com",
                Username = "test",
                Name = "test test"
            });

            var actual = await userService.GetUsersAsync();

            Assert.AreEqual(expected.Count, actual.Count());
            Assert.AreEqual(expected.FirstOrDefault().Id, actual.FirstOrDefault().Id);
            Assert.AreEqual(expected.FirstOrDefault().Email, actual.FirstOrDefault().Email);
            Assert.AreEqual(expected.FirstOrDefault().Username, actual.FirstOrDefault().Username);
            Assert.AreEqual(expected.FirstOrDefault().Name, actual.FirstOrDefault().Name);
        }

        [Test]
        public async Task UpdateUserSuccessfully()
        {
            var userService = serviceProvider.GetService<IUserService>();

            var input = new UserEditViewModel()
            {
                Id = "test1234-test-test-test-test1234test",
                FirstName = "Viktor",
                LastName = "Tsolov"
            };

            var result = await userService.UpdateUserAsync(input);

            Assert.AreEqual(true, result);
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
                    Id = "310b8a7e-734d-44d6-b3f3-efa6e8f6259d",
                    UserName = "test1",
                    Email = "email@gmail.com"
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
                    Email = "abv1234@gmail.com",
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
