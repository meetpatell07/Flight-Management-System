using System.ComponentModel.DataAnnotations;

namespace GBC_Travel_Group_54.Models
{
    public class Passenger
    {

        [Key]
        public int PassengerId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

       
    }
}
