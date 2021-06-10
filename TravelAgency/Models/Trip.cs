using System;
using System.Collections.Generic;

namespace TravelAgency.Models
{
    public class Trip
    {
        public int Id { get; set; }

        public double Price { get; set; }

        public int DeparturePlaceId { get; set; }
        public Place DeparturePlace { get; set; }

        public int ArrivalPlaceId { get; set; }
        public Place ArrivalPlace { get; set; }

        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }

        public List<Ticket> Tickets { get; set; }
    }
}