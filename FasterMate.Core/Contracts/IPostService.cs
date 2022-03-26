namespace FasterMate.Core.Contracts
{
    using FasterMate.ViewModels.Post;

    public interface IPostService
    {
        Task CreateAsync(string id, CreatePostViewModel input, string path);

        IEnumerable<RenderProfilePostsViewModel> RenderPostsForProfile(string id);
    }
}
