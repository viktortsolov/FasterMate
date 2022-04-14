namespace FasterMate.Test
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using FasterMate.Core.Contracts;
    using FasterMate.Core.Services;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;
    using FasterMate.Infrastructure.Data.Enums;
    using FasterMate.ViewModels.Comment;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;

    public class CommentServiceTest
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
                .AddSingleton<ICommentService, CommentService>()
                .BuildServiceProvider();

            var commentRepo = serviceProvider.GetService<IRepository<Comment>>();
            var postRepo = serviceProvider.GetService<IRepository<Post>>();
            var imgRepo = serviceProvider.GetService<IRepository<Image>>();
            var profileRepo = serviceProvider.GetService<IRepository<Profile>>();
            var countryRepo = serviceProvider.GetService<IRepository<Country>>();

            await SeedDb(commentRepo, postRepo, imgRepo, profileRepo, countryRepo);
        }

        [Test]
        public async Task AddAsyncSuccessfully()
        {
            var service = serviceProvider.GetService<ICommentService>();
            var commentRepo = serviceProvider.GetService<IRepository<Comment>>();

            var input = new AddCommentViewModel()
            {
                PostId = "737e89d8-f182-4588-94be-71fab80cba1b",
                Comment = "Some test text"
            };

            await service.AddAsync("6747d8c2-dfc9-40d3-864c-4cb28bff6038", input);

            var expected = "6747d8c2-dfc9-40d3-864c-4cb28bff6038";
            var actual = commentRepo.All().Skip(1).FirstOrDefault().ProfileId;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task RemoveAsyncSuccessfully()
        {
            var service = serviceProvider.GetService<ICommentService>();

            var actual = await service.DeleteAsync("e92097e5-ba0d-494d-bc8d-0f9ddb85b3e7");
            var expected = "737e89d8-f182-4588-94be-71fab80cba1b";

            Assert.AreEqual(actual, expected);
        }

        private static async Task SeedDb(IRepository<Comment> commentRepo, IRepository<Post> postRepo, IRepository<Image> imgRepo, IRepository<Profile> profileRepo, IRepository<Country> countryRepo)
        {
            await countryRepo.AddAsync(new Country() { Id = "test1234-test-test-test-test1234test", Name = "test" });
            await imgRepo.AddAsync(new Image() { Id = "test1234-test-test-test-test1234test", Extension = "test" });
            await countryRepo.SaveChangesAsync();
            await imgRepo.SaveChangesAsync();

            await commentRepo.AddAsync(new Comment()
            {
                Id = "e92097e5-ba0d-494d-bc8d-0f9ddb85b3e7",
                Text = "Some test text",
                ProfileId = "6747d8c2-dfc9-40d3-864c-4cb28bff6038",
                PostId = "737e89d8-f182-4588-94be-71fab80cba1b",
                CreatedOn = DateTime.UtcNow
            });

            await postRepo.AddAsync(new Post()
            {
                Id = "737e89d8-f182-4588-94be-71fab80cba1b",
                Text = "Some test text",
                Location = "Some test location",
                CreatedOn = DateTime.UtcNow,
                ImageId = imgRepo.AllAsNoTracking().Select(x => x.Id).FirstOrDefault(),
                ProfileId = "6747d8c2-dfc9-40d3-864c-4cb28bff6038"
            });

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

            await commentRepo.SaveChangesAsync();
        }
    }
}
