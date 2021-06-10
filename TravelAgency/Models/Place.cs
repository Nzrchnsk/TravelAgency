using System.Collections.Generic;

namespace TravelAgency.Models
{
    public class Place
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public string Address { get; set; }

        public List<Trip> ArrivalTrips { get; set; }
        public List<Trip> DepartureTrips { get; set; }
    }
}