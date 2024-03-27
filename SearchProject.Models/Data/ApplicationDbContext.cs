using Microsoft.EntityFrameworkCore;

namespace SearchProject.Models.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Search> Searches { get; set; }
        public DbSet<SearchResult> SearchResults { get; set; }
    }
}
