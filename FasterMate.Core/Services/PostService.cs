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
        private readonly IRepository<Profile> profileRepo;

        public PostService(
            IImageService _imgService,
            ICommentService _commentService,
            IRepository<Post> _postRepo,
            IRepository<PostLike> _postLikesRepo,
            IRepository<Comment> _commentRepo,
            IRepository<Profile> _profileRepo)
        {
            imgService = _imgService;
            commentService = _commentService;

            postRepo = _postRepo;
            postLikesRepo = _postLikesRepo;
            commentRepo = _commentRepo;
            profileRepo = _profileRepo;
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
                .Include(x => x.Profile)
                .Where(x => x.Id == id)
                .Select(x => new RenderSeePostViewModel()
                {
                    Id = x.Id,
                    ProfileName = $"{x.Profile.FirstName} {x.Profile.LastName}",
                    ProfileImgPath = x.Profile.Image != null ? $"{x.Profile.Image.Id}.{x.Profile.Image.Extension}" : null,
                    ProfileId = x.ProfileId,
                    Text = x.Text,
                    ImagePath = $"{x.ImageId}.{x.Image.Extension}",
                    CreatedOn = x.CreatedOn.ToString("dd/MM/yyyy"),
                    LikesCount = postLikesRepo.All().Where(x => x.PostId == id).Count(),
                    CommentsCount = commentRepo.All().Where(x => x.PostId == id).Count(),
                    Comments = commentRepo
                        .All()
                        .Include(x => x.Profile)
                        .Where(x => x.PostId == id)
                        .Select(x => new RenderCommentViewModel()
                        {
                            CommentId = x.Id,
                            PostId = x.PostId,
                            ProfileId = x.ProfileId,
                            ProfileName = $"{x.Profile.FirstName} {x.Profile.LastName}",
                            Text = x.Text,
                            CreatedOn = x.CreatedOn.ToString("dd/MM/yyyy")
                        }).ToList(),
                    //commentService.GetAllOfPost(id), IDK why this is not working...
                    Location = x.Location
                })
                .FirstOrDefault();

            return post;
        }

        public IEnumerable<RenderTimelinePostsViewModel> RenderTimelinePosts()
           => postRepo
            .AllAsNoTracking()
            .Include(x => x.Image)
            .Include(x => x.Profile)
            .OrderByDescending(x => x.CreatedOn)
            .Select(r => new RenderTimelinePostsViewModel()
            {
                Id = r.Id,
                Text = r.Text,
                Location = r.Location,
                CreatedOn = r.CreatedOn.ToString("dd/MM/yyyy"),
                ImagePath = $"{r.Image.Id}.{r.Image.Extension}",
                LikesCount = postLikesRepo.All().Where(x => x.PostId == r.Id).Count(),
                CommentsCount = commentRepo.All().Where(x => x.PostId == r.Id).Count(),
                ProfileId = r.ProfileId,
                ProfileName = $"{r.Profile.FirstName} {r.Profile.LastName}",
                ProfileImgPath = r.Profile.Image != null ? $"{r.Profile.Image.Id}.{r.Profile.Image.Extension}" : null
            })
            .ToList();
    }
}
