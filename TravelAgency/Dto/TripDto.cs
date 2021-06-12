using System;

namespace TravelAgency.Dto
{
    public class TripDto
    {
        public double Price { get; set; }
        public int DeparturePlaceId { get; set; }
        public int ArrivalPlaceId { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public int  TotalTicket { get; set; }
    }
}