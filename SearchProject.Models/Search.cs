using System.ComponentModel.DataAnnotations;

namespace SearchProject.Models
{
    public class Search : BaseEntity<Guid>
    {
        [Required]
        [MaxLength(255)]
        public string Url { get; set; }

        [Required]
        [MaxLength(255)]
        public string Keywords { get; set; }

        public ICollection<SearchResult> Results { get; set; }

        public DateTime CreatedOnUtc { get; set; }
    }
}
