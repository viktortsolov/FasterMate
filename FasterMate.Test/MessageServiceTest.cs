namespace FasterMate.Test
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FasterMate.Core.Contracts;
    using FasterMate.Core.Services;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;

    using NUnit.Framework;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using FasterMate.Infrastructure.Data.Enums;
    using System.Linq;
    using FasterMate.ViewModels.Message;

    public class MessageServiceTest
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
                .AddSingleton<IMessageService, MessageService>()
                .BuildServiceProvider();

            var profileRepo = serviceProvider.GetService<IRepository<Profile>>();
            var countryRepo = serviceProvider.GetService<IRepository<Country>>();
            var imgRepo = serviceProvider.GetService<IRepository<Image>>();
            var groupRepo = serviceProvider.GetService<IRepository<Group>>();
            var msgRepo = serviceProvider.GetService<IRepository<Message>>();

            await SeedDb(profileRepo, countryRepo, imgRepo, groupRepo, msgRepo);
        }

        [Test]
        public async Task AddMessageSuccessfully()
        {
            var msgRepo = serviceProvider.GetService<IRepository<Message>>();
            var msgService = serviceProvider.GetService<IMessageService>();

            string groupId = "test1234-test-test-test-test1234test";
            string profileId = "6747d8c2-dfc9-40d3-864c-4cb28bff6038";
            string text = "some test text";

            string actual = await msgService.AddMessageAsync(groupId, profileId, text);
            string expected = msgRepo.All().Skip(1).FirstOrDefault().Id;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetMessageByIdSuccessfully()
        {
            var expected = new MessageViewModel()
            {
                Id = "test1234-test-test-test-test1234test",
                Text = "some test text",
                ProfileId = "6747d8c2-dfc9-40d3-864c-4cb28bff6038",
                CreatedOn = DateTime.UtcNow.ToString(),
                ProfileName = "test test"
            };

            var msgService = serviceProvider.GetService<IMessageService>();

            var actual = msgService.GetMessageById("test1234-test-test-test-test1234test");
            
            Assert.AreEqual(expected.ProfileId, actual.ProfileId);
        }

        private static async Task SeedDb(IRepository<Profile> profileRepo, IRepository<Country> countryRepo, IRepository<Image> imgRepo, IRepository<Group> groupRepo, IRepository<Message> msgRepo)
        {
            await countryRepo.AddAsync(new Country() { Id = "test1234-test-test-test-test1234test", Name = "test" });
            await countryRepo.SaveChangesAsync();

            await imgRepo.AddAsync(new Image() { Id = "test1234-test-test-test-test1234test", Extension = "test" });
            await imgRepo.SaveChangesAsync();

            await profileRepo.AddAsync(new Profile()
            {
                Id = "6747d8c2-dfc9-40d3-864c-4cb28bff6038",
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
            await profileRepo.SaveChangesAsync();


            await groupRepo.AddAsync(new Group()
            {
                Id = "test1234-test-test-test-test1234test",
                Name = "Test Group Name",
                ImageId = "test1234-test-test-test-test1234test",
                ProfileId = "6747d8c2-dfc9-40d3-864c-4cb28bff6038"
            });
            await groupRepo.SaveChangesAsync();

            await msgRepo.AddAsync(new Message()
            {
                Id = "test1234-test-test-test-test1234test",
                Text = "some test text",
                GroupId = "test1234-test-test-test-test1234test",
                ProfileId = "6747d8c2-dfc9-40d3-864c-4cb28bff6038",
                CreateOn = DateTime.UtcNow
            });
            await msgRepo.SaveChangesAsync();
        }
    }
}
