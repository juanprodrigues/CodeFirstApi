using CodeFirstApi.Services.Interfaces;
using DB;
using Microsoft.AspNetCore.Mvc;

namespace CodeFirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeerController : ControllerBase
    {
        private readonly IBeerService _beerService;

        public BeerController(IBeerService beerService)
        {
            _beerService = beerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Beer>>> Get()
        {
            return Ok(await _beerService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Beer>> Get(int id)
        {
            var beer = await _beerService.GetByIdAsync(id);

            if (beer == null)
                return NotFound();

            return Ok(beer);
        }

        [HttpPost]
        public async Task<ActionResult<Beer>> Post(Beer beer)
        {
            var createdBeer = await _beerService.AddAsync(beer);

            return CreatedAtAction(nameof(Get), new { id = createdBeer.BeerID }, createdBeer);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Beer beer)
        {
            var updatedBeer = await _beerService.UpdateAsync(id, beer);

            if (updatedBeer == null)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _beerService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}