namespace FasterMate.Core.Contracts
{
    using FasterMate.ViewModels.Comment;

    public interface ICommentService
    {
        Task AddAsync(string profileId, AddCommentViewModel input);

        Task DeleteAsync(string id);

        IEnumerable<RenderCommentViewModel> GetAllOfPost(string postId);
    }
}
