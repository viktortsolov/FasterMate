namespace FasterMate.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using FasterMate.Core.Contracts;
    using FasterMate.Core.Services;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;
    using FasterMate.Infrastructure.Data.Enums;
    using FasterMate.ViewModels.Comment;
    using FasterMate.ViewModels.Group;
    using FasterMate.ViewModels.Post;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;

    public class PostServiceTest
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
                .AddSingleton<IImageService, ImageService>()
                .AddSingleton<IPostService, PostService>()
                .BuildServiceProvider();

            await SeedDb();
        }

        [Test]
        public async Task CreateSuccessfully()
        {
            var postRepo = serviceProvider.GetService<IRepository<Post>>();
            var postService = serviceProvider.GetService<IPostService>();

            using (var stream = File.OpenRead("test.png"))
            {
                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/png"
                };

                var input = new CreatePostViewModel()
                {
                    ReturnId = "test",
                    Text = "test text post",
                    Image = file,
                    Location = "Somewhere!"
                };

                await postService.CreateAsync("test", input, $"{Directory.GetCurrentDirectory()}");
            }

            Assert.AreEqual(2, postRepo.All().Count());
        }

        [Test]
        public async Task LikePostSuccessfully()
        {
            var postLikesRepo = serviceProvider.GetService<IRepository<PostLike>>();
            var postService = serviceProvider.GetService<IPostService>();

            await postService.LikePostAsync("test", "testPost");

            Assert.AreEqual(2, postLikesRepo.All().Count());
        }

        [Test]
        public async Task UnLikePostSuccessfully()
        {
            var postLikesRepo = serviceProvider.GetService<IRepository<PostLike>>();
            var postService = serviceProvider.GetService<IPostService>();

            await postService.LikePostAsync("test", "testPost");
            await postService.LikePostAsync("test", "testPost");

            Assert.AreEqual(1, postLikesRepo.All().Count());
        }

        [Test]
        public void RenderSinglePostSuccessfully()
        {
            var postService = serviceProvider.GetService<IPostService>();

            var commentsExpected = new List<RenderCommentViewModel>();
            commentsExpected.Add(new RenderCommentViewModel()
            {
                CommentId = "test",
                PostId = "testPost",
                ProfileId = "test",
                ProfileName = "test test",
                Text = "some text",
                CreatedOn = DateTime.Now.ToString("HH:mm, dd/MM/yyyy")
            });

            var postExpected = new RenderSeePostViewModel()
            {
                Id = "testPost",
                ProfileName = "test test",
                ProfileImgPath = "test.test",
                IsOwner = true,
                IsLikedByVisitor = false,
                Text = "test text",
                ImagePath = "test.test",
                Location = "somewhere",
                CreatedOn = DateTime.Now.ToString("HH:mm, dd/MM/yyyy"),
                LikesCount = 0,
                CommentsCount = 1,
                Comments = commentsExpected
            };

            var actual = postService.RenderSinglePost("testPost", "test");

            Assert.AreEqual(postExpected.Comments.FirstOrDefault().Text, actual.Comments.FirstOrDefault().Text);

        }

        [Test]
        public void RenderTimelinePostsSuccessfully()
        {
            var postService = serviceProvider.GetService<IPostService>();

            var expected = new List<RenderTimelinePostsViewModel>();
            expected.Add(new RenderTimelinePostsViewModel()
            {

                Id = "testPost",
                Text = "test text",
                Location = "somewhere",
                CreatedOn = DateTime.Now.ToString("hh:mm, dd/MM/yyyy"),
                ImagePath = "test.test",
                LikesCount = 0,
                CommentsCount = 1,
                ProfileId = "test",
                ProfileName = "test test",
                ProfileImgPath = "test.test"
            });

            var actual = postService.RenderTimelinePosts();

            Assert.AreEqual(expected.FirstOrDefault().CommentsCount, actual.FirstOrDefault().CommentsCount);
        }

        [Test]
        public async Task DeleteWithCommentsSuccessfully()
        {
            var postRepo = serviceProvider.GetService<IRepository<Post>>();
            var postService = serviceProvider.GetService<IPostService>();

            await postService.DeleteAsync("test", "testPost");

            Assert.AreEqual(0, postRepo.All().Count());
        }

        [Test]
        public async Task DeleteWithLikesSuccessfully()
        {
            var postRepo = serviceProvider.GetService<IRepository<Post>>();
            var postService = serviceProvider.GetService<IPostService>();

            await postService.LikePostAsync("test", "testPost");
            await postService.DeleteAsync("test", "testPost");

            Assert.AreEqual(0, postRepo.All().Count());
        }

        [Test]
        public async Task DeleteFromAdministatorSuccessfully()
        {
            var postRepo = serviceProvider.GetService<IRepository<Post>>();
            var postService = serviceProvider.GetService<IPostService>();

            await postService.DeletePostAdministratorAsync("testPost");

            Assert.AreEqual(0, postRepo.All().Count());
        }

        [Test]
        public async Task PostListAdministratorSuccessfully()
        {
            var postService = serviceProvider.GetService<IPostService>();

            var expected = new PostListViewModel()
            {
                Id = "testPost",
                ProfileId = "test",
                CreatedOn = DateTime.Now.ToString("hh:mm, dd/MM/yyyy"),
                Name = "test test",
                LikesCount = 1,
                CommentsCount = 1
            };

            var actual = await postService.PostListAdministratorAsync();

            Assert.AreEqual(expected.LikesCount, actual.FirstOrDefault().LikesCount);
            Assert.AreEqual(expected.CommentsCount, actual.FirstOrDefault().CommentsCount);
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
            var postRepo = serviceProvider.GetService<IRepository<Post>>();
            var commentRepo = serviceProvider.GetService<IRepository<Comment>>();
            var postLikesRepo = serviceProvider.GetService<IRepository<PostLike>>();

            await countryRepo.AddAsync(new Country() { Id = "test", Name = "test" });
            await countryRepo.SaveChangesAsync();

            var image = new Image() { Id = "test", Extension = "test" };
            await imgRepo.AddAsync(image);
            await imgRepo.SaveChangesAsync();

            var profile = new Profile()
            {
                Id = "test",
                FirstName = "test",
                LastName = "test",
                CountryId = countryRepo.AllAsNoTracking().Select(x => x.Id).FirstOrDefault(),
                BirthDate = DateTime.UtcNow,
                Gender = Gender.Male,
                User = new ApplicationUser()
                {
                    Id = "test"
                },
                ImageId = imgRepo.AllAsNoTracking().Select(x => x.Id).FirstOrDefault(),
            };

            var secondProfile = new Profile()
            {
                Id = "test1",
                FirstName = "test1",
                LastName = "test1",
                CountryId = countryRepo.AllAsNoTracking().Select(x => x.Id).FirstOrDefault(),
                BirthDate = DateTime.UtcNow,
                Gender = Gender.Male,
                User = new ApplicationUser()
                {
                    Id = "test1"
                },
                ImageId = imgRepo.AllAsNoTracking().Select(x => x.Id).FirstOrDefault(),
            };

            await profileRepo.AddAsync(profile);
            await profileRepo.AddAsync(secondProfile);
            await profileRepo.SaveChangesAsync();

            var post = new Post()
            {
                Id = "testPost",
                Text = "test text",
                Location = "somewhere",
                CreatedOn = DateTime.Now,
                Image = image,
                Profile = profile
            };
            await postRepo.AddAsync(post);
            await postRepo.SaveChangesAsync();

            var comment = new Comment()
            {
                Id = "test",
                Post = post,
                Profile = profile,
                Text = "some text",
                CreatedOn = DateTime.Now,
            };
            await commentRepo.AddAsync(comment);
            await commentRepo.SaveChangesAsync();

            await postLikesRepo.AddAsync(new PostLike()
            {
                Profile = secondProfile,
                ProfileId = secondProfile.Id,
                Post = post,
                PostId = post.Id
            });
            await postLikesRepo.SaveChangesAsync();
        }
    }
}
