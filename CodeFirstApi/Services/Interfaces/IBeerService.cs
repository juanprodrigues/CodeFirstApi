using DB;

namespace CodeFirstApi.Services.Interfaces
{
    public interface IBeerService
    {
        Task<IEnumerable<Beer>> GetAllAsync();
        Task<Beer?> GetByIdAsync(int id);
        Task<Beer> AddAsync(Beer beer);
        Task<Beer?> UpdateAsync(int id, Beer beer);
        Task<bool> DeleteAsync(int id);
    }
}