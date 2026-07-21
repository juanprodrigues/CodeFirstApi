using DB;
using Microsoft.EntityFrameworkCore;
using CodeFirstApi.Services.Interfaces;
using CodeFirstApi.Repositories.Interfaces;
namespace CodeFirstApi.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _repository;

        public BrandService(IBrandRepository repository)
        {
            _repository = repository;
        }


        public async Task<IEnumerable<Brand>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }


        public async Task<Brand?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }


        public async Task<Brand> AddAsync(Brand Brand)
        {
            await _repository.AddAsync(Brand);

            return await _repository.GetByIdAsync(Brand.BrandID);
        }


        public async Task<Brand?> UpdateAsync(int id, Brand Brand)
        {
            var currentBrand = await _repository.GetByIdAsync(id);

            if (currentBrand == null)
                return null;


            currentBrand.intName = Brand.intName;
            currentBrand.BrandID = Brand.BrandID;


            await _repository.UpdateAsync(currentBrand);


            return await _repository.GetByIdAsync(id);
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var Brand = await _repository.GetByIdAsync(id);

            if (Brand == null)
                return false;


            await _repository.DeleteAsync(Brand);

            return true;
        }
    }
}