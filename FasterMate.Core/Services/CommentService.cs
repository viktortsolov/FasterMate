namespace FasterMate.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FasterMate.Core.Contracts;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;
    using FasterMate.ViewModels.Comment;

    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> commentRepo;
        private readonly IRepository<Post> postRepo;

        public CommentService(IRepository<Comment> _commentRepo,
            IRepository<Post> _postRepo)
        {
            commentRepo = _commentRepo;
            postRepo = _postRepo;
        }

        public async Task AddAsync(string profileId, AddCommentViewModel input)
        {
            var comment = new Comment()
            {
                ProfileId = profileId,
                PostId = input.PostId,
                Text = input.Comment
            };

            await commentRepo.AddAsync(comment);
            await commentRepo.SaveChangesAsync();
        }

        public async Task<string> DeleteAsync(string id)
        {
            var comment = commentRepo
                .All()
                .Where(x => x.Id == id)
                .FirstOrDefault();

            var postId = comment.PostId;

            if (comment != null)
            {
                commentRepo.Delete(comment);
                await commentRepo.SaveChangesAsync();
            }

            return postId;
        }
        
        //TODO: Try to fix it
        public IEnumerable<RenderCommentViewModel> GetAllOfPost(string id)
            => commentRepo
                        .All()
                        .Where(x => x.PostId == id)
                        .Select(x => new RenderCommentViewModel()
                        {
                            CommentId = x.Id,
                            PostId = x.PostId,
                            ProfileId = x.ProfileId,
                            Text = x.Text,
                            CreatedOn = x.CreatedOn.ToString("dd/MM/yyyy")
                        }).ToList();
    }
}
