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
    [Route("api/trips")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAsyncRepository<Trip> _tripRepository;
        private readonly IMapper _mapper;


        public TripController(ApplicationDbContext context, IAsyncRepository<Trip> tripRepository, IMapper mapper)
        {
            _mapper = mapper;
            _tripRepository = tripRepository;
            _context = context;
        }

        // GET: api/Trip
        [AllowAnonymous]
        [HttpGet]
        public async Task<List<TripResponseDto>> GetTrips() => await GetTrip().ToListAsync();


        // GET: api/Trip/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TripResponseDto>> GetTrip(int id)
        {
            var result = await _context.Trips
                .Include(i => i.ArrivalPlace)
                .Include(i => i.DeparturePlace)
                .Include(i => i.Tickets)
                .FirstOrDefaultAsync(t => t.Id == id);
            var trip = new TripResponseDto(result);
            if (result == null)
            {
                return NotFound();
            }

            return trip;
        }

        private IQueryable<TripResponseDto> GetTrip() => _context.Trips
            .Include(i => i.ArrivalPlace)
            .Include(i => i.DeparturePlace)
            .Include(i => i.Tickets)
            .Select(n => new TripResponseDto(n));

        // PUT: api/Trip/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrip(int id, TripDto request)
        {
            if (request.ArrivalPlaceId == request.DeparturePlaceId)
            {
                return BadRequest();
            }

            var trip = await _tripRepository.GetByIdAsync(id);
            if (trip is null)
            {
                return NotFound();
            }

            _mapper.Map(request, trip);

            try
            {
                await _tripRepository.UpdateAsync(trip);
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
                return await _tripRepository.AddAsync(trip);
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
            var trip = await _tripRepository.GetByIdAsync(id);
            if (trip == null)
            {
                return NotFound();
            }

            await _tripRepository.DeleteAsync(trip);

            return NoContent();
        }

        private bool TripExists(int id) => _context.Trips.Any(e => e.Id == id);
    }
}