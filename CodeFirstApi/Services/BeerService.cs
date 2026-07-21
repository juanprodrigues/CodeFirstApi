using CodeFirstApi.Services.Interfaces;
using DB;
using Microsoft.EntityFrameworkCore;

namespace CodeFirstApi.Services
{
    public class BeerService : IBeerService
    {
        private readonly BarContext _context;

        public BeerService(BarContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Beer>> GetAllAsync()
        {
            return await _context.Beers
                .Include(x => x.Brand)
                .ToListAsync();
        }

        public async Task<Beer?> GetByIdAsync(int id)
        {
            return await _context.Beers
                .Include(x => x.Brand)
                .FirstOrDefaultAsync(x => x.BeerID == id);
        }

        public async Task<Beer> AddAsync(Beer beer)
        {
            _context.Beers.Add(beer);
            await _context.SaveChangesAsync();

            // Cargar la relación Brand para la respuesta
            await _context.Entry(beer)
                .Reference(x => x.Brand)
                .LoadAsync();

            return beer;
        }

        public async Task<Beer?> UpdateAsync(int id, Beer beer)
        {
            var currentBeer = await _context.Beers
                .Include(x => x.Brand)
                .FirstOrDefaultAsync(x => x.BeerID == id);

            if (currentBeer == null)
                return null;

            currentBeer.intName = beer.intName;
            currentBeer.BrandID = beer.BrandID;

            await _context.SaveChangesAsync();

            // Recargar la nueva Brand si cambió el BrandID
            await _context.Entry(currentBeer)
                .Reference(x => x.Brand)
                .LoadAsync();

            return currentBeer;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var beer = await _context.Beers.FindAsync(id);

            if (beer == null)
                return false;

            _context.Beers.Remove(beer);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}