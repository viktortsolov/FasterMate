namespace FasterMate.Test
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using FasterMate.Core.Contracts;
    using FasterMate.Core.Services;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;

    public class ImageServiceTest
    {
        private readonly string imgLocation = $"{Directory.GetCurrentDirectory()}";

        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;

        [SetUp]
        public void Setup()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollecton = new ServiceCollection();

            serviceProvider = serviceCollecton
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton(typeof(IRepository<>), typeof(Repository<>))
                .AddSingleton<IImageService, ImageService>()
                .BuildServiceProvider();
        }

        [Test]
        public async Task AddImageWorksSuccessfully()
        {
            string imgLocaion = $"{Directory.GetCurrentDirectory()}";

            var image = new Image()
            {
                Id = "6ec02675-9fc2-4572-904c-229e0190129f",
                Extension = "png"
            };

            var imgRepo = serviceProvider.GetService<IRepository<Image>>();
            var imgService = serviceProvider.GetService<IImageService>();

            await imgRepo.AddAsync(image);
            await imgRepo.SaveChangesAsync();

            using (var stream = File.OpenRead("test.png"))
            {
                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/png"
                };

                await imgService.CreateAsync(file, imgLocaion);
            }

            Assert.AreEqual(2, imgRepo.All().Count());
        }
    }
}
