using System.ComponentModel.DataAnnotations;

namespace GBC_Travel_Group_54.Models
{
    public class Amenity
    {
        [Key] 
        public int AmenityId { get; set; }

        public string AmenityName { get; set; } 

    }
}
