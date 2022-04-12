namespace FasterMate.ViewModels.Group
{
    public class GroupMemberViewModel
    {
        public string Id { get; set; }

        public IEnumerable<GroupMemberInfoViewModel> Members { get; set; }
    }
}
