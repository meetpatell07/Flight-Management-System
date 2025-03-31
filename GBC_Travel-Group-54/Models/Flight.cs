using System.ComponentModel.DataAnnotations;

namespace GBC_Travel_Group_54.Models
{
    public class Flight
    {
        [Key]
        [Required]
        public int FlightId { get; set; }

        /* [Required]
         public string Airline { get; set; }*/

        [Required]
        public string DepartureAirport { get; set; }

        [Required]
        public string ArrivalAirport { get; set; }

        [Required]
        public string Airline { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DepartureDateTime { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime ArrivalDateTime { get; set; }

        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public decimal Price { get; set; }
    }
}
