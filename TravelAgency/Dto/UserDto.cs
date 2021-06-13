using TravelAgency.Models;

namespace TravelAgency.Dto
{
    public class UserDto
    {
        public UserDto(User user, string role)
        {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            Role = role;
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}