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

        private readonly IRepository<Post> postRepo;
        private readonly IRepository<PostLike> postLikesRepo;
        private readonly IRepository<Comment> commentRepo;

        public PostService(
            IImageService _imgService,
            IRepository<Post> _postRepo,
            IRepository<PostLike> _postLikesRepo,
            IRepository<Comment> _commentRepo)
        {
            imgService = _imgService;

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

            if (input.Image.Length > 0)
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
                .Select(r => new RenderProfilePostsViewModel()
                {
                    Id = r.Id,
                    Text = r.Text,
                    Location = r.Location,
                    CreatedOn = r.CreatedOn.ToString("HH:mm, dd/MM/yyyy"),
                    ImagePath = $"{r.Image.Id}.{r.Image.Extension}",
                    LikesCount = postLikesRepo.All().Where(x => x.PostId == r.Id).Count(),
                    CommentsCount = commentRepo.All().Where(x => x.PostId == r.Id).Count()
                })
                .ToList();

        public RenderSeePostViewModel RenderSinglePost(string id, string profileId)
        {
            var comments = commentRepo
                    .All()
                    .Include(x => x.Profile)
                    .ThenInclude(x => x.Image)
                    .Where(x => x.PostId == id)
                    .OrderByDescending(x => x.CreatedOn)
                    .Select(x => new RenderCommentViewModel()
                    {
                        CommentId = x.Id,
                        PostId = x.PostId,
                        ProfileId = x.ProfileId,
                        Gender = x.Profile.Gender.ToString(),
                        ImagePath = x.Profile.Image != null ? $"{x.Profile.Image.Id}.{x.Profile.Image.Extension}" : null,
                        ProfileName = $"{x.Profile.FirstName} {x.Profile.LastName}",
                        Text = x.Text,
                        CreatedOn = x.CreatedOn.ToString("HH:mm, dd/MM/yyyy")
                    })
                    .ToList();

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
                    IsOwner = x.ProfileId == profileId,
                    Gender = x.Profile.Gender.ToString(),
                    IsLikedByVisitor = postLikesRepo.All().Any(x => x.PostId == id && x.ProfileId == profileId),
                    Text = x.Text,
                    ImagePath = $"{x.ImageId}.{x.Image.Extension}",
                    CreatedOn = x.CreatedOn.ToString("HH:mm, dd/MM/yyyy"),
                    LikesCount = postLikesRepo.All().Where(x => x.PostId == id).Count(),
                    CommentsCount = commentRepo.All().Where(x => x.PostId == id).Count(),
                    Comments = comments,
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
                CreatedOn = r.CreatedOn.ToString("hh:mm, dd/MM/yyyy"),
                ImagePath = $"{r.Image.Id}.{r.Image.Extension}",
                LikesCount = postLikesRepo.All().Where(x => x.PostId == r.Id).Count(),
                CommentsCount = commentRepo.All().Where(x => x.PostId == r.Id).Count(),
                ProfileId = r.ProfileId,
                ProfileName = $"{r.Profile.FirstName} {r.Profile.LastName}",
                Gender = r.Profile.Gender.ToString(),
                ProfileImgPath = r.Profile.Image != null ? $"{r.Profile.Image.Id}.{r.Profile.Image.Extension}" : null
            })
            .ToList();

        public async Task DeleteAsync(string profileId, string id)
        {
            var post = postRepo
                .All()
                .Where(x => x.Id == id && x.ProfileId == profileId)
                .FirstOrDefault();

            if (post != null)
            {
                foreach (var comment in commentRepo.All().Where(x => x.PostId == id))
                {
                    commentRepo.Delete(comment);
                }

                foreach (var postLike in postLikesRepo.All().Where(x => x.PostId == id))
                {
                    postLikesRepo.Delete(postLike);
                }

                postRepo.Delete(post);
                await postRepo.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<PostListViewModel>> PostListAdministratorAsync()
            => await postRepo
                .AllAsNoTracking()
                .Include(x => x.Profile)
                .Include(x => x.PostLikes)
                .Include(x => x.Comments)
                .Select(x => new PostListViewModel()
                {
                    Id = x.Id,
                    ProfileId = x.ProfileId,
                    CreatedOn = x.CreatedOn.ToString("hh:mm, dd/MM/yyyy"),
                    Name = $"{x.Profile.FirstName} {x.Profile.LastName}",
                    LikesCount = x.PostLikes.Count,
                    CommentsCount = x.Comments.Count
                })
                .ToListAsync();

        public async Task DeletePostAdministratorAsync(string id)
        {
            var post = postRepo
                .All()
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (post != null)
            {
                foreach (var comment in commentRepo.All().Where(x => x.PostId == id))
                {
                    commentRepo.Delete(comment);
                }

                foreach (var postLike in postLikesRepo.All().Where(x => x.PostId == id))
                {
                    postLikesRepo.Delete(postLike);
                }

                postRepo.Delete(post);
                await postRepo.SaveChangesAsync();
            }
        }
    }
}