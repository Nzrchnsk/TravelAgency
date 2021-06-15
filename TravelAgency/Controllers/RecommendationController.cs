using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Dto;
using TravelAgency.Helper;
using TravelAgency.Interfaces;
using TravelAgency.Models;

namespace TravelAgency.Controllers
{
    [Route("api/recommendation")]
    public class RecommendationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public RecommendationController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Recommendation([FromBody]RecommendationDto reqeust)
        {
            var _user = await _userManager.FindByIdAsync(reqeust.User.ToString());

            var _trip = await _context.Trips
                .Include(i => i.ArrivalPlace)
                .Include(i => i.DeparturePlace)
                .FirstOrDefaultAsync(t => t.Id == reqeust.Trip);

            if (_user is null || _trip is null) return NotFound();
            
            await EmailHelper.Recommendation(_user.Email, _trip, reqeust.Text);
            return Ok();
        }
    }
}