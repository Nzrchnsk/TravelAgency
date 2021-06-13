using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Specification.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Dto;
using TravelAgency.Helper;
using TravelAgency.Initializers;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/tickets")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IAsyncRepository<Ticket> _ticketRepository;
        private readonly IMapper _mapper;


        public TicketController(ApplicationDbContext context, UserManager<User> userManager,
            IAsyncRepository<Ticket> ticketRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userManager = userManager;
            _context = context;
            _ticketRepository = ticketRepository;
        }

        // GET: api/Ticket
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            if (User.IsInRole(Rolse.Admin))
            {
                return await _context.Tickets
                    .Include(t => t.Trip)
                    .Include(t => t.Trip.DeparturePlace)
                    .Include(t => t.Trip.ArrivalPlace)
                    .Include(t => t.User).Select(t => new Ticket
                    {
                        Id = t.Id,
                        Trip = t.Trip,
                        Number = t.Number,
                        User = new User
                        {
                            UserName = t.User.UserName
                        }
                    }).ToListAsync();
            }
            else
            {
                var user = await CurrentUser();
                return await _context.Tickets
                    .Include(t => t.Trip)
                    .Include(t => t.Trip.DeparturePlace)
                    .Include(t => t.Trip.ArrivalPlace)
                    .Include(t => t.User)
                    .Where(t => t.UserId == user.Id)
                    .Select(t => new Ticket
                    {
                        Id = t.Id,
                        Trip = t.Trip,
                        Number = t.Number,
                        User = new User
                        {
                            UserName = t.User.UserName
                        }
                    }).ToListAsync();
            }
        }

        // GET: api/Ticket/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Trip)
                .Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null || await IsUserOwner(ticket))
            {
                return NotFound();
            }

            return ticket;
        }

        // PUT: api/Ticket/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(int id, TicketDto request)
        {
            if (!TripExist(request.TripId)) return NotFound();
            var ticket = await _ticketRepository.GetByIdAsync(id);

            if (await IsUserOwner(ticket))
            {
                try
                {
                    _mapper.Map(request, ticket);
                    await _ticketRepository.UpdateAsync(ticket);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return NoContent();
        }

        // POST: api/Ticket
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostTicket(TicketDto request)
        {
            if (!TripExist(request.TripId)) return NotFound();
            try
            {
                var trip = _context.Trips
                    .Include(t => t.ArrivalPlace)
                    .Include(t => t.DeparturePlace)
                    .Include(t => t.Tickets)
                    .First(t => t.Id == request.TripId);
                if (trip.TotalTicket == trip.Tickets.Count) return BadRequest();
                var user = await CurrentUser();
                var ticket = _mapper.Map<Ticket>(request);
                ticket.UserId = user.Id;
                var result = await _ticketRepository.AddAsync(ticket);
                result.Number = $"T-{new string('0', 6 - result.Id.ToString().Length)}{result.Id}";
                await _ticketRepository.UpdateAsync(result);
                await EmailHelper.BuyTicket(user.Email, result.Number,
                    new[] {trip.DeparturePlace.Name, trip.ArrivalPlace.Name});
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Ticket/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null || !await IsUserOwner(ticket))
            {
                return NotFound();
            }

            await _ticketRepository.DeleteAsync(ticket);

            return NoContent();
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }

        private async Task<bool> IsUserOwner(Ticket ticket)
        {
            if (User.IsInRole(Rolse.Admin)) return true;
            var user = await CurrentUser();
            return ticket.UserId == user.Id;
        }

        private async Task<User> CurrentUser()
        {
            return await _userManager.FindByEmailAsync(User.Identity?.Name);
        }

        private bool TripExist(int tripId) => _context.Trips.Any(trip => trip.Id == tripId);
    }
}