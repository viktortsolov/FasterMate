namespace FasterMate.ViewModels.Group
{
    using FasterMate.ViewModels.Message;

    public class GroupViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }
        
        public string ProfileId { get; set; }

        public IEnumerable<MessageViewModel> Messages { get; set; }
    }
}
