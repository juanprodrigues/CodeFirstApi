using DB;

namespace CodeFirstApi.Repositories.Interfaces
{

    public interface IBrandRepository
    {

        Task<IEnumerable<Brand>> GetAllAsync();
        Task<Brand?> GetByIdAsync(int id);
        Task AddAsync(Brand Brand);
        Task UpdateAsync(Brand Brand);
        Task DeleteAsync(Brand Brand);
    }
}