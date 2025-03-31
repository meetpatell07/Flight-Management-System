using System.ComponentModel.DataAnnotations;

namespace GBC_Travel_Group_54.Models
{
    public class Hotel
    {
        [Key]
        public int HotelId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int NumGuest { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public decimal PricingPerNight { get; set; }

        [Required]
        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5")]
        public int Rating { get; set; }

    }
}
