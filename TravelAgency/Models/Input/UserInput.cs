using TravelAgency.Dto;

namespace TravelAgency.Models.Input
{
    public class UserInput : LoginDto
    {
        public string UserName { get; set; }
        
    }
}