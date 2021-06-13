using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Dto;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.Controllers
{
    [Route("api/places")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlaceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAsyncRepository<Place> _placesRepository;
        private readonly IMapper _mapper;

        public PlaceController(ApplicationDbContext context, IMapper mapper, IAsyncRepository<Place> placesRepository)
        {
            _placesRepository = placesRepository;
            _mapper = mapper;
            _context = context;
        }

        // GET: api/Place
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Place>>> GetPlaces()
        {
            return await _context.Places.ToListAsync();
        }

        // GET: api/Place/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Place>> GetPlace(int id)
        {
            var place = await _placesRepository.GetByIdAsync(id);

            if (place == null)
            {
                return NotFound();
            }

            return place;
        }

        // PUT: api/Place/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlace(int id, PlaceDto request)
        {
            var place = await _placesRepository.GetByIdAsync(id);
            _mapper.Map(request, place);
            try
            {
                await _placesRepository.UpdateAsync(place);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Place
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Place>> PostPlace(PlaceDto request)
        {
            var place = _mapper.Map<Place>(request);
            return await _placesRepository.AddAsync(place);
        }

        // DELETE: api/Place/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlace(int id)
        {
            var place = await _placesRepository.GetByIdAsync(id);
            if (place == null)
            {
                return NotFound();
            }

            await _placesRepository.DeleteAsync(place);

            return NoContent();
        }

        private bool PlaceExists(int id)
        {
            return _context.Places.Any(e => e.Id == id);
        }
    }
}