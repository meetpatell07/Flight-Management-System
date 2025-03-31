using System.ComponentModel.DataAnnotations;

namespace GBC_Travel_Group_54.Models
{
    public class Car
    {
        [Key]
        public int CarId { get; set; }

        [Required]
        [StringLength(50)]
        public string Make { get; set; }

        [Required]
        [StringLength(50)]
        public string Model { get; set; }

        [Required]
        public int NumPassenger { get; set; }

        [Required]
        [StringLength(4)]
        public string Year { get; set; }

        [Required]
        [StringLength(100)]
        public string RentalCompanies { get; set; }

        [Required]
        public decimal RentalPricePerDay { get; set; }
        public bool IsAvailable { get; set; }
    }
}
