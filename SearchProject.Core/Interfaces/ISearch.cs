using SearchProject.Models;

namespace SearchProject.Core.Interfaces
{
    public interface ISearch
    {
        Task<Search> GetById(Guid id);

        Task<IEnumerable<Search>> GetAll();

        Task<Search> Save(Search search);
    }
}
