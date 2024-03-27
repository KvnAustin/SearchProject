using Microsoft.EntityFrameworkCore;
using SearchProject.Core.Interfaces;
using SearchProject.Models;
using SearchProject.Models.Data;

namespace SearchProject.Core.Services
{
    public class SearchService : ISearch
    {
        private readonly ApplicationDbContext _context;

        public SearchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Search>> GetAll()
            => await Query()
                .ToListAsync();

        public async Task<Search> GetById(Guid id)
            => await Query()
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Search> Save(Search search)
        {
            search.CreatedOnUtc = DateTime.UtcNow;

            _context.Searches.Add(search);

            await _context.SaveChangesAsync();

            return search;
        }

        #region [ Helper Methods ]
        private IQueryable<Search> Query()
            => _context.Searches
                .Include(x => x.Results)
                .AsQueryable();
        #endregion
    }
}
