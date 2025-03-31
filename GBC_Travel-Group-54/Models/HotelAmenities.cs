using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GBC_Travel_Group_54.Models
{
    public class HotelAmenities
    {
        [Key]
        public int HotelAmenitiesId { get; set; }

        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }

        public int AmenityId { get; set; }
        public Amenity Amenity { get; set; }
    }

}
