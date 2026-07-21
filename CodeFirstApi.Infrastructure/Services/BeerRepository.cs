using CodeFirstApi.Repositories.Interfaces;
using DB;
using Microsoft.EntityFrameworkCore;

namespace CodeFirstApi.Repositories
{
    public class BeerRepository : IBeerRepository
    {
        private readonly BarContext _context;

        public BeerRepository(BarContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Beer>> GetAllAsync()
        {
            return await _context.Beers
                .Include(x => x.Brand)
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task<Beer?> GetByIdAsync(int id)
        {
            return await _context.Beers
                .Include(x => x.Brand)
                .FirstOrDefaultAsync(x => x.BeerID == id);
        }


        public async Task AddAsync(Beer beer)
        {
            await _context.Beers.AddAsync(beer);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(Beer beer)
        {
            _context.Beers.Update(beer);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteAsync(Beer beer)
        {
            _context.Beers.Remove(beer);
            await _context.SaveChangesAsync();
        }
    }
}