using DB;

namespace CodeFirstApi.Repositories.Interfaces
{
    public interface IBeerRepository
    {
        Task<IEnumerable<Beer>> GetAllAsync();
        Task<Beer?> GetByIdAsync(int id);
        Task AddAsync(Beer beer);
        Task UpdateAsync(Beer beer);
        Task DeleteAsync(Beer beer);
    }
}