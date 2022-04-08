namespace FasterMate.ViewModels.Home
{
    public class SearchViewModel
    {
        public string SearchText { get; set; }

        public IEnumerable<ProfileSearchViewModel> Profiles { get; set; }
    }
}
