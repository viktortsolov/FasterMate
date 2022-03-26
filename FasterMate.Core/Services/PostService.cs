namespace FasterMate.Core.Services
{
    using System.Threading.Tasks;

    using FasterMate.Core.Contracts;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;
    using FasterMate.ViewModels.Post;

    public class PostService : IPostService
    {
        private readonly IImageService imgService;

        private readonly IRepository<Post> postRepo;

        public PostService(
            IImageService _imgService,
            IRepository<Post> _postRepo)
        {
            imgService = _imgService;

            postRepo = _postRepo;
        }

        public async Task CreateAsync(string id, CreatePostViewModel input, string path)
        {
            var post = new Post()
            {
                ProfileId = id,
                Text = input.Text,
                Location = input.Location
            };

            if (input.Image?.Length > 0)
            {
                post.ImageId = await imgService.CreateAsync(input.Image, path);
            }

            await postRepo.AddAsync(post);
            await postRepo.SaveChangesAsync();
        }
    }
}
