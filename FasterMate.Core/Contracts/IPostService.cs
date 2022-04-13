﻿namespace FasterMate.Core.Contracts
{
    using FasterMate.ViewModels.Post;

    public interface IPostService
    {
        Task CreateAsync(string id, CreatePostViewModel input, string path);

        IEnumerable<RenderProfilePostsViewModel> RenderPostsForProfile(string id);

        Task LikePostAsync(string profileId, string postId);

        RenderSeePostViewModel RenderSinglePost(string id, string profileId);

        IEnumerable<RenderTimelinePostsViewModel> RenderTimelinePosts();

        Task DeleteAsync(string profileId, string id);

        Task<IEnumerable<PostListViewModel>> PostListAdministratorAsync();

        Task DeletePostAdministratorAsync(string id);
    }
}
