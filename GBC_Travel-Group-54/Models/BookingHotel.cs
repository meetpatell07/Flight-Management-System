using System;
using System.ComponentModel.DataAnnotations;

namespace GBC_Travel_Group_54.Models
{
    public class BookingHotel
    {
        public int BookingHotelId { get; set; }

        [Required]
        public int HotelId { get; set; }

        [Required]
        public int PassengerId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CheckInDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CheckOutDate { get; set; }

        // Add other properties as needed

        // Navigation properties
        public Hotel? Hotel { get; set; }
        public Passenger? Passenger { get; set; }
    }
}
