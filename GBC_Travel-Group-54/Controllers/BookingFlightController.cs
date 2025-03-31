using GBC_Travel_Group_54.Data;
using GBC_Travel_Group_54.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GBC_Travel_Group_54.Controllers
{
    public class BookingFlightController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingFlightController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var bookings = _context.Bookingflights
                .Include(bf => bf.Passenger)
                .Include(bf => bf.Flight)
                .ToList();

            return View(bookings);
        }
        
        

        [HttpGet]
        public IActionResult CreateForm(int flightId, int passengerId)
        {
            var flight = _context.Flights.Find(flightId);
            var passenger = _context.Passengers.Find(passengerId);

            if (flight == null || passenger == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var bookingFlight = new BookingFlight
            {
                FlightId = flightId,
                PassengerId = passengerId,
            };

            bookingFlight.Flight = flight;
            bookingFlight.Passenger = passenger;

            ViewData["FlightDetails"] = flight;
            ViewData["PassengerDetails"] = passenger;

            return View("CreateForm", bookingFlight);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookingFlight bookingFlight)
        {
            if (ModelState.IsValid)
            {
                _context.Bookingflights.Add(bookingFlight);
                _context.SaveChanges();

                return RedirectToAction("BookingConfirm", new { bookingId = bookingFlight.BookingFlightId });
            }

            return View("Create", bookingFlight);
        }


        public IActionResult BookingConfirm(int bookingId)
        {
            BookingFlight bookingDetails = _context.Bookingflights.Include(bf => bf.Flight)
                .Include(bf => bf.Passenger)
                .FirstOrDefault(b => b.BookingFlightId == bookingId);

            if (bookingDetails == null)
            {
                return RedirectToAction("Index", "Flight"); // Redirect to home for example
            }

            return View(bookingDetails);
        }





        public IActionResult ViewBooking(int bookingId)
        {
            BookingFlight bookingDetails = _context.Bookingflights.FirstOrDefault(b => b.BookingFlightId == bookingId);
            if (bookingDetails == null)
            {
                return RedirectToAction("Index", "Home"); // Redirect to home for example
            }

            return View("ViewBooking", bookingDetails);
        }


    }
}