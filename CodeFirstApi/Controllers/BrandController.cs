using DB;
using Microsoft.AspNetCore.Mvc;
using CodeFirstApi.Services.Interfaces;

namespace CodeFirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _BrandService;

        public BrandController(IBrandService BrandService)
        {
            _BrandService = BrandService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> Get()
        {
            return Ok(await _BrandService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> Get(int id)
        {
            var Brand = await _BrandService.GetByIdAsync(id);

            if (Brand == null)
                return NotFound();

            return Ok(Brand);
        }

        [HttpPost]
        public async Task<ActionResult<Brand>> Post(Brand Brand)
        {
            var createdBrand = await _BrandService.AddAsync(Brand);

            return CreatedAtAction(nameof(Get), new { id = createdBrand.BrandID }, createdBrand);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Brand Brand)
        {
            var updatedBrand = await _BrandService.UpdateAsync(id, Brand);

            if (updatedBrand == null)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _BrandService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}