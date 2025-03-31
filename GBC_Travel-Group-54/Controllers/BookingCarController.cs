using GBC_Travel_Group_54.Data;
using GBC_Travel_Group_54.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GBC_Travel_Group_54.Controllers
{
    public class BookingCarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingCarController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var bookings = _context.bookingCars
                .Include(bc => bc.Passenger)
                .Include(bc => bc.Car)
                .ToList();

            return View(bookings);
        }

        public IActionResult BookCar(int carId)
        {
            // Pass the car ID to the Passenger Create view
            var car = _context.Cars.Find(carId);

            // Redirect to the Passenger controller's Create action with car details
            return RedirectToAction("CreateForCar", "Passenger", new
            {
                carId = carId,
            });
        }

        [HttpGet]
        public IActionResult CreateForm(int carId, int passengerId)
        {
            // Retrieve the car and passenger details from the database
            var car = _context.Cars.Find(carId);
            var passenger = _context.Passengers.Find(passengerId);

            if (car == null || passenger == null)
            {
                // Handle the case where car or passenger is not found (e.g., redirect, show an error message)
                return RedirectToAction("Index", "Home");
            }

            // Create a new BookingCar object and populate its properties
            var bookingCar = new BookingCar
            {
                CarId = carId,
                PassengerId = passengerId,
                PickUpDate = DateTime.Today,
                DropOffDate = DateTime.Today.AddDays(1),
                PickUpLocation = "",
                DropOffLocation = ""
                // Set other properties of booking car as needed
            };

            bookingCar.Car = car;
            bookingCar.Passenger = passenger;

            // Pass the car, passenger, and booking car details to the view
            ViewData["CarDetails"] = car;
            ViewData["PassengerDetails"] = passenger;

            return View("CreateForm", bookingCar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookingCar bookingCar)
        {
            // Process the POST request to create the booking
            if (ModelState.IsValid)
            {
                // Save the booking details in the database
                _context.bookingCars.Add(bookingCar);
                _context.SaveChanges();

                // Redirect to the booking confirmation action with the bookingId
                return RedirectToAction("BookingConfirm", new { bookingId = bookingCar.BookingCarId });
            }

            // If model state is not valid, return to the same view with validation errors
            return View("Create", bookingCar);
        }

        public IActionResult BookingConfirm(int bookingId)
        {
            // Retrieve the booking details from the database based on the bookingId
            BookingCar bookingDetails = _context.bookingCars
                .Include(bc => bc.Car)
                .Include(bc => bc.Passenger)
                .FirstOrDefault(bc => bc.BookingCarId == bookingId);

            if (bookingDetails == null)
            {
                // Handle the case where the bookingId doesn't exist (redirect, show an error, etc.)
                return RedirectToAction("Index", "Car"); // Redirect to home for example
            }

            // Pass the booking details to the view
            return View(bookingDetails);
        }

        public IActionResult ViewBooking(int bookingId)
        {
            // Retrieve the booking details from the database based on the bookingId
            BookingCar bookingDetails = _context.bookingCars.FirstOrDefault(bc => bc.BookingCarId == bookingId);
            if (bookingDetails == null)
            {
                // Handle the case where the bookingId doesn't exist (redirect, show an error, etc.)
                return RedirectToAction("Index", "Home"); // Redirect to home for example
            }

            // Pass the booking details to the view
            return View("ViewBooking", bookingDetails);
        }



    }
}
