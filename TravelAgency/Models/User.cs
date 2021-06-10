using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TravelAgency.Models
{
    public class User: IdentityUser<int>
    {
        public List<Order> Orders { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}