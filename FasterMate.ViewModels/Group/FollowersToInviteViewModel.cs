namespace FasterMate.ViewModels.Group
{
    public class FollowersToInviteViewModel
    {
        public string Id { get; set; }

        public IEnumerable<GroupFollowersViewModel> Followers { get; set; }
    }
}
