using Microsoft.AspNetCore.Mvc;

namespace TravelAgency.Models
{
    public class Ticket : BaseEntity
    {
        public string Number { get; set; }

        public int TripId { get; set; }
        public Trip Trip { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        // public int OrderId { get; set; }
        // public Order Order { get; set; }

        public string Status { get; set; }
    }
}