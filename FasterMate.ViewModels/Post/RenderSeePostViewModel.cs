namespace FasterMate.ViewModels.Post
{
    using FasterMate.ViewModels.Comment;

    public class RenderSeePostViewModel
    {
        public string Id { get; set; }

        public string ProfileId { get; set; }

        public bool IsOwner { get; set; }

        public bool IsLikedByVisitor { get; set; }

        public string ProfileName { get; set; }

        public string ProfileImgPath { get; set; }

        public string Text { get; set; }

        public string Location { get; set; }

        public string ImagePath { get; set; }

        public string CreatedOn { get; set; }

        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }

        public string Gender { get; set; }

        public IEnumerable<RenderCommentViewModel> Comments { get; set; } = new HashSet<RenderCommentViewModel>();

        public AddCommentViewModel Comment { get; set; }

        public string PostId { get; set; }

        public string ReturnId { get; set; }

    }
}
