namespace FasterMate.ViewModels.Group
{
    public class ProfileGroupsViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        public int GroupMembersCount { get; set; }

        public bool IsOwner { get; set; }
    }
}
