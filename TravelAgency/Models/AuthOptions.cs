using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TravelAgency.Models
{
    public class AuthOptions
    {
        public const string ISSUER = "TestApp"; // издатель токена
        public const string AUDIENCE = "TestAppClient"; // потребитель токена
        const string KEY = "sdlfnadnfjkuuooknshucwx";   // ключ для шифрации
        public const int LIFETIME = 360; // время жизни токена в минутах
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }

    }
}