using CodeFirstApi.Services.Interfaces;
using DB;
using Microsoft.EntityFrameworkCore;

namespace CodeFirstApi.Services
{
    public class BrandService : IBrandService
    {
        private readonly BarContext _context;

        public BrandService(BarContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Brand>> GetAllAsync()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task<Brand?> GetByIdAsync(int id)
        {
            return await _context.Brands.FindAsync(id);
        }

        public async Task<Brand> AddAsync(Brand Brand)
        {
            _context.Brands.Add(Brand);
            await _context.SaveChangesAsync();

            return Brand;
        }

        public async Task<Brand?> UpdateAsync(int id, Brand Brand)
        {
            var currentBrand = await _context.Brands.FindAsync(id);

            if (currentBrand == null)
                return null;

            currentBrand.intName = Brand.intName;
            currentBrand.BrandID = Brand.BrandID;

            await _context.SaveChangesAsync();

            return currentBrand;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var Brand = await _context.Brands.FindAsync(id);

            if (Brand == null)
                return false;

            _context.Brands.Remove(Brand);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}