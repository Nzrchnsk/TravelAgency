using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TravelAgency.Initializers
{
    public static class RoleInitializer
    {
        public static async Task InitializeRoleAsync(RoleManager<IdentityRole<int>> roleManager)
        {
            var roles = Rolse.AllRoles;
            foreach (var role in roles)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole<int>(role));
                }
            }
        }
    }
}