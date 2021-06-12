using System.ComponentModel.DataAnnotations;

namespace TravelAgency.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}