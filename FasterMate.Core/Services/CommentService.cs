namespace FasterMate.Core.Services
{
    using FasterMate.Core.Contracts;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;
    using FasterMate.ViewModels.Comment;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> commentRepo;

        public CommentService(IRepository<Comment> _commentRepo)
        {
            commentRepo = _commentRepo;
        }

        public async Task AddAsync(string profileId, AddCommentViewModel input)
        {
            var comment = new Comment()
            {
                ProfileId = profileId,
                PostId = input.PostId,
                Text = input.Text
            };

            await commentRepo.AddAsync(comment);
            await commentRepo.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var comment = commentRepo
                .All()
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (comment != null)
            {
                commentRepo.Delete(comment);
                await commentRepo.SaveChangesAsync();
            }
        }

        public IEnumerable<RenderCommentViewModel> GetAllOfPost()
        {
            throw new NotImplementedException();
        }
    }
}
