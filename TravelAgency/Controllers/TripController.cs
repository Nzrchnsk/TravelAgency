using System;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/trip")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAsyncRepository<Trip> _placesRepository;
        private readonly IMapper _mapper;


        public TripController(ApplicationDbContext context, IAsyncRepository<Trip> placesRepository, IMapper mapper)
        {
            _mapper = mapper;
            _placesRepository = placesRepository;
            _context = context;
        }

        // GET: api/Trip
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trip>>> GetTrips()
        {
            return await _context.Trips.ToListAsync();
        }

        // GET: api/Trip/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Trip>> GetTrip(int id)
        {
            var trip = await _placesRepository.GetByIdAsync(id);

            if (trip == null)
            {
                return NotFound();
            }

            return trip;
        }

        // PUT: api/Trip/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrip(int id, TripDto request)
        {
            if (request.ArrivalPlaceId == request.DeparturePlaceId)
            {
                return BadRequest();
            }

            var trip = await _placesRepository.GetByIdAsync(id);
            if (trip is null)
            {
                return NotFound();
            }

            _mapper.Map(request, trip);

            try
            {
                await _placesRepository.UpdateAsync(trip);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Trip
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Trip>> PostTrip(TripDto request)
        {
            var trip = _mapper.Map<Trip>(request);
            try
            {
                return await _placesRepository.AddAsync(trip);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Trip/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(int id)
        {
            var trip = await _placesRepository.GetByIdAsync(id);
            if (trip == null)
            {
                return NotFound();
            }

            await _placesRepository.DeleteAsync(trip);

            return NoContent();
        }

        private bool TripExists(int id)
        {
            return _context.Trips.Any(e => e.Id == id);
        }
    }
}