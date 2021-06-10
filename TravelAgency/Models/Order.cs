using System;
using System.Collections.Generic;

namespace TravelAgency.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int Status { get; set; }

        public List<Ticket> Tickets { get; set; }
    }
}