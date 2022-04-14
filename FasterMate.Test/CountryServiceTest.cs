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

    public class CountryServiceTest
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
                .AddSingleton<ICountryService, CountryService>()
                .BuildServiceProvider();

            var countryRepo = serviceProvider.GetService<IRepository<Country>>();

            await SeedDb(countryRepo);
        }

        [Test]
        public void GetAllAsKvpSuccessfully()
        {
            var countryService = serviceProvider.GetService<ICountryService>();

            var expected = new List<KeyValuePair<string, string>>();

            expected.Add(new KeyValuePair<string, string>("test", "test"));
            expected.Add(new KeyValuePair<string, string>("0c2fb2cb-8932-4755-b63f-e4664c0526e7", "test2"));
            expected.Add(new KeyValuePair<string, string>("0c2fb2cb-8932-4755-b63f-e4664c0526e7", "test3"));

            var actual = countryService.GetAllAsKvp();

            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private async Task SeedDb(IRepository<Country> countryRepo)
        {
            await countryRepo.AddAsync(new Country() { Id = "test", Name = "test" });
            await countryRepo.AddAsync(new Country() { Id = "0c2fb2cb-8932-4755-b63f-e4664c0526e7", Name = "test2" });
            await countryRepo.AddAsync(new Country() { Id = "0c8533b3-92ec-448f-b4bc-8ee5084db978", Name = "test3" });

            await countryRepo.SaveChangesAsync();
        }
    }
}
