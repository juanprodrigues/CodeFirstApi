using DB;

namespace CodeFirstApi.Services.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<Brand>> GetAllAsync();
        Task<Brand?> GetByIdAsync(int id);
        Task<Brand> AddAsync(Brand Brand);
        Task<Brand?> UpdateAsync(int id, Brand Brand);
        Task<bool> DeleteAsync(int id);
    }
}