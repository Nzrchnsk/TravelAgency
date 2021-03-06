using System;
using System.Collections.Generic;

namespace TravelAgency.Models
{
    public class Trip : BaseEntity
    {
        public double Price { get; set; }
        public int DeparturePlaceId { get; set; }
        public Place DeparturePlace { get; set; }
        public int ArrivalPlaceId { get; set; }
        public Place ArrivalPlace { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public int  TotalTicket { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}