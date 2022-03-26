namespace FasterMate.ViewModels.Profile
{
    using FasterMate.ViewModels.Post;

    public class RenderProfileViewModel
    {
        public string Id { get; set; }

        public bool IsOwner { get; set; }

        public bool IsFollowing { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public DateTime Birthdate { get; set; }

        public string Bio { get; set; }

        public string ImagePath { get; set; }

        public string Country { get; set; }

        public int FollowingCount { get; set; }

        public int FollowersCount { get; set; }

        public IEnumerable<RenderProfilePostsViewModel> Posts { get; set; } = new HashSet<RenderProfilePostsViewModel>();
    }
}
