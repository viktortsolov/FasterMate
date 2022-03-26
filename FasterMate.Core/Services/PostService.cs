namespace FasterMate.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FasterMate.Core.Contracts;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;
    using FasterMate.ViewModels.Comment;
    using FasterMate.ViewModels.Post;

    using Microsoft.EntityFrameworkCore;

    public class PostService : IPostService
    {
        private readonly IImageService imgService;
        private readonly ICommentService commentService;

        private readonly IRepository<Post> postRepo;
        private readonly IRepository<PostLike> postLikesRepo;
        private readonly IRepository<Comment> commentRepo;

        public PostService(
            IImageService _imgService,
            ICommentService _commentService,
            IRepository<Post> _postRepo,
            IRepository<PostLike> _postLikesRepo,
            IRepository<Comment> _commentRepo)
        {
            imgService = _imgService;
            commentService = _commentService;

            postRepo = _postRepo;
            postLikesRepo = _postLikesRepo;
            commentRepo = _commentRepo;
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

        public async Task LikePostAsync(string profileId, string postId)
        {
            var postLike = postLikesRepo
                .All()
                .FirstOrDefault(x => x.PostId == postId && x.ProfileId == profileId);

            if (postLike == null)
            {
                postLike = new PostLike()
                {
                    ProfileId = profileId,
                    PostId = postId
                };

                await postLikesRepo.AddAsync(postLike);
            }
            else
            {
                postLikesRepo.Delete(postLike);
            }

            await postLikesRepo.SaveChangesAsync();
        }

        public IEnumerable<RenderProfilePostsViewModel> RenderPostsForProfile(string id)
            => postRepo
                .AllAsNoTracking()
                .Include(x => x.Image)
                .Where(x => x.ProfileId == id)
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => new RenderProfilePostsViewModel()
                {
                    Id = x.Id,
                    Text = x.Text,
                    Location = x.Location,
                    CreatedOn = x.CreatedOn.ToString("dd/MM/yyyy"),
                    ImagePath = $"{x.Image.Id}.{x.Image.Extension}",
                    LikesCount = postLikesRepo.All().Where(x => x.PostId == id).Count(),
                    CommentsCount = commentRepo.All().Where(x => x.PostId == id).Count()
                })
                .ToList();

        public RenderSeePostViewModel RenderSinglePost(string id)
        {
            var post = postRepo
                .AllAsNoTracking()
                .Include(x => x.Image)
                .Where(x => x.Id == id)
                .Select(x => new RenderSeePostViewModel()
                {
                    Id = x.Id,
                    Text = x.Text,
                    ImagePath = $"{x.ImageId}.{x.Image.Extension}",
                    CreatedOn = x.CreatedOn.ToString("dd/MM/yyyy"),
                    LikesCount = postLikesRepo.All().Where(x => x.PostId == id).Count(),
                    CommentsCount = commentRepo.All().Where(x => x.PostId == id).Count(),
                    Comments = commentRepo
                        .All()
                        .Where(x => x.PostId == id)
                        .Select(x => new RenderCommentViewModel()
                        {
                            CommentId = x.Id,
                            PostId = x.PostId,
                            ProfileId = x.ProfileId,
                            Text = x.Text,
                            CreatedOn = x.CreatedOn.ToString("dd/MM/yyyy")
                        }).ToList()
                })
                .FirstOrDefault();

            return post;
        }
    }
}
