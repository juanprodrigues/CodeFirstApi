using CodeFirstApi.Repositories.Interfaces;
using DB;
using Microsoft.EntityFrameworkCore;

namespace CodeFirstApi.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly BarContext _context;

        public BrandRepository(BarContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Brand>> GetAllAsync()
        {
            return await _context.Brands
                .Include(x => x.Beers)
                .ToListAsync();
        }

        public async Task<Brand?> GetByIdAsync(int id)
        {
            return await _context.Brands
                .Include(x => x.Beers)
                .FirstOrDefaultAsync(x => x.BrandID == id);
        }

        public async Task AddAsync(Brand brand)
        {
            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Brand brand)
        {
            _context.Brands.Update(brand);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Brand brand)
        {
            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
        }
    }
}