using System.ComponentModel.DataAnnotations;

namespace GBC_Travel_Group_54.Models
{
    public class BookingCar
    {
        [Key]
        public int BookingCarId { get; set; }

        [Required]
        public int CarId { get; set; }

        [Required]
        public int PassengerId { get; set; }

        [Required]
        public string PickUpLocation { get; set; }

        [Required]
        public string DropOffLocation { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime PickUpDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DropOffDate { get; set; }

        // Additional passenger information
        public Passenger? Passenger { get; set; }
        public Car? Car { get; set; }

    }
}
