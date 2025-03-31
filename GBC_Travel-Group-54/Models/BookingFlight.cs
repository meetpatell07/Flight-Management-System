using System.ComponentModel.DataAnnotations;
namespace GBC_Travel_Group_54.Models
{
    public class BookingFlight
    {
        [Key]
        public int BookingFlightId { get; set; }

        [Required]
        public int PassengerId { get; set; }

        [Required]
        public int FlightId { get; set; }

        // Additional properties
        public Passenger? Passenger { get; set; }
        public Flight? Flight { get; set; }
    }
}
