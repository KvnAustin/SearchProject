namespace SearchProject.UI.ViewModels
{
    public class SearchViewModel : BaseViewModel<Guid>
    {
        public string Url { get; set; }

        public string Keywords { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public List<int> Indexes { get; set; }
    }
}
