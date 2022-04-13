namespace FasterMate.ViewModels.Post
{
    public class PostListViewModel
    {
        public string Id { get; set; }

        public string ProfileId { get; set; }

        public string Name { get; set; }

        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }

        public string CreatedOn { get; set; }
    }
}
