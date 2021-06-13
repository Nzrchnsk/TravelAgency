using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Models;

namespace TravelAgency.Initializers
{
    public class MigrationInitializer
    {
        
        private readonly ApplicationDbContext _applicationDbContext;

        public MigrationInitializer(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task Run()
        {
            await _applicationDbContext.Database.MigrateAsync();
            var places = _applicationDbContext.Places;
            if (!places.Any())
            {
                places.Add(new Place
                {
                    Name = "Тесть 1",
                    Description = "фывфывфывфывфыв",
                    Address = "йцуыячсячс",
                });
                places.Add(new Place
                {
                    Name = "Тесть 2",
                    Description = "фывфывфывфывфыв",
                    Address = "йцуыячсячс"
                });
                await _applicationDbContext.SaveChangesAsync();
            }
        }
    }
}