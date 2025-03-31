using GBC_Travel_Group_54.Models;
using Microsoft.EntityFrameworkCore;

namespace GBC_Travel_Group_54.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSet properties for your model classes
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Passenger> Passengers { get; set; }

        public DbSet<BookingFlight> Bookingflights { get; set; }

        public DbSet<Car> Cars { get; set; }
        public DbSet<BookingCar> bookingCars { get; set; }


        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<BookingHotel> BookingHotels { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed predefined amenities
            modelBuilder.Entity<Amenity>().HasData(
                new Amenity { AmenityId = 1, AmenityName = "Pool" },
                new Amenity { AmenityId = 2, AmenityName = "Breakfast" },
                new Amenity { AmenityId = 3, AmenityName = "Gym" },
                new Amenity { AmenityId = 4, AmenityName = "WiFi" },
                new Amenity { AmenityId = 5, AmenityName = "Parking" },
                new Amenity { AmenityId = 6, AmenityName = "Housekeeping" },
                new Amenity { AmenityId = 7, AmenityName = "Bar" },
                new Amenity { AmenityId = 8, AmenityName = "Dinner" }
            );
        }


        public DbSet<HotelAmenities> HotelAmenities { get; set; }




    }
}
