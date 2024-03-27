using SearchProject.Models.Data;

namespace SearchProject.Models
{
    public class SearchResult : BaseEntity<Guid>
    {
        public Guid SearchId { get; set; }
        public Search Search { get; set; }

        public int Index { get; set; }
    }
}
