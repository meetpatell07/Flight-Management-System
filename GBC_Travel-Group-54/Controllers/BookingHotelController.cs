using GBC_Travel_Group_54.Data;
using GBC_Travel_Group_54.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GBC_Travel_Group_54.Controllers
{
    public class BookingHotelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingHotelController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var bookings = _context.BookingHotels
                .Include(bh => bh.Passenger)
                .Include(bh => bh.Hotel)
                .ToList();

            return View(bookings);
        }

       

        [HttpGet]
        public IActionResult CreateForm(int hotelId, int passengerId)
        {
            var hotel = _context.Hotels.Find(hotelId);
            var passenger = _context.Passengers.Find(passengerId);

            if (hotel == null || passenger == null)
            {
                // Handle the case where flight or passenger is not found (e.g., redirect, show an error message)
                return RedirectToAction("About", "Home");
            }

            var bookingHotel = new BookingHotel
            {
                HotelId = hotelId,
                PassengerId = passengerId,
                CheckInDate = DateTime.Today,
                CheckOutDate = DateTime.Today.AddDays(1)
            };
            bookingHotel.Hotel = hotel;
            bookingHotel.Passenger = passenger;

            // Pass the flight, passenger, and booking flight details to the view
            ViewData["HotelDetails"] = hotel;
            ViewData["PassengerDetails"] = passenger;

            return View("CreateForm", bookingHotel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookingHotel bookingHotel)
        {
            // Process the POST request to create the booking
            if (ModelState.IsValid)
            {
                // Save the booking details in the database
                _context.BookingHotels.Add(bookingHotel);
                _context.SaveChanges();

                // Redirect to the booking confirmation action with the bookingId
                return RedirectToAction("BookingConfirm", new { bookingId = bookingHotel.BookingHotelId });
            }

            // If model state is not valid, return to the same view with validation errors
            return View("Ddd", bookingHotel);
        }

        public IActionResult BookingConfirm(int bookingId)
        {
            // Retrieve the booking details from the database based on the bookingId
            BookingHotel bookingDetails = _context.BookingHotels.Include(bh => bh.Hotel)
                           .Include(bf => bf.Passenger)
                           .FirstOrDefault(b => b.BookingHotelId == bookingId);
            if (bookingDetails == null)
            {
                // Handle the case where the bookingId doesn't exist (redirect, show an error, etc.)
                return RedirectToAction("Index", "Home"); // Redirect to home for example
            }

            // Pass the booking details to the view
            return View(bookingDetails);
        }

        public IActionResult ViewBookings(int bookingId)
        {
            BookingHotel bookingDetails = _context.BookingHotels.FirstOrDefault(bc => bc.BookingHotelId == bookingId);
            if (bookingDetails == null)
            {
                // Handle the case where the bookingId doesn't exist (redirect, show an error, etc.)
                return RedirectToAction("Index", "Home"); // Redirect to home for example
            }

            return View("ViewBooking", bookingDetails);
        }
    }
}