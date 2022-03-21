namespace FasterMate.ViewModels.Profile
{
    public class RenderProfileViewModel
    {
        public RenderProfileViewModel()
        {
            Posts = new List<string>();
        }

        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public DateTime Birthdate { get; set; }

        public string Bio { get; set; }

        public string ImagePath { get; set; }

        public string Country { get; set; }

        public int FollowersCount { get; set; }

        public int FollowingCount { get; set; }

        //TODO: Posts
        public IEnumerable<string> Posts { get; set; }
    }
}
