using TravelAgency.Models;

namespace TravelAgency.Dto
{
    public class TripResponseDto : TripDto
    {
        public TripResponseDto()
        {
            
        }
        public TripResponseDto(Trip trip)
        {
            Id = trip.Id;
            Price = trip.Price;
            Name = trip.ArrivalPlace.Name.Trim() + " - " + trip.DeparturePlace.Name.Trim();
            TotalTicket = trip.TotalTicket;
            FreePlaces = trip.TotalTicket - trip.Tickets.Count;
            DeparturePlace = trip.DeparturePlace.Name;
            DepartureDate = trip.DepartureDate;
            DeparturePlaceId = trip.DeparturePlaceId;
            ArrivalPlaceId = trip.ArrivalPlaceId;
            ArrivalPlace = trip.ArrivalPlace.Name;
            ArrivalDate = trip.ArrivalDate;
        }
        public int Id { get; private set; }
        public int FreePlaces { get; set; }
        public string Name { get; set; }
        public string DeparturePlace { get; set; }
        public string ArrivalPlace { get; set; }
    }
}