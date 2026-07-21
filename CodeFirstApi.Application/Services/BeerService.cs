using CodeFirstApi.Repositories.Interfaces;
using CodeFirstApi.Services.Interfaces;
using DB;

namespace CodeFirstApi.Services
{
    public class BeerService : IBeerService
    {
        private readonly IBeerRepository _repository;

        public BeerService(IBeerRepository repository)
        {
            _repository = repository;
        }


        public async Task<IEnumerable<Beer>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }


        public async Task<Beer?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }


        public async Task<Beer> AddAsync(Beer beer)
        {
            await _repository.AddAsync(beer);

            return await _repository.GetByIdAsync(beer.BeerID);
        }


        public async Task<Beer?> UpdateAsync(int id, Beer beer)
        {
            var currentBeer = await _repository.GetByIdAsync(id);

            if (currentBeer == null)
                return null;


            currentBeer.intName = beer.intName;
            currentBeer.BrandID = beer.BrandID;


            await _repository.UpdateAsync(currentBeer);


            return await _repository.GetByIdAsync(id);
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var beer = await _repository.GetByIdAsync(id);

            if (beer == null)
                return false;


            await _repository.DeleteAsync(beer);

            return true;
        }
    }
}