using GBC_Travel_Group_54.Data;
using GBC_Travel_Group_54.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_Group54.Controllers
{
    public class PassengerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PassengerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(int flightId)
        {
            var tasks = _context.Passengers
                .ToList();

            ViewBag.FlightId = flightId;
            return View(tasks);
        }

        public IActionResult Details(int id)
        {
            var passenger = _context.Passengers.FirstOrDefault(p => p.PassengerId == id);
            if (passenger == null)
            {
                return NotFound();
            }
            return View(passenger);
        }

        public IActionResult Create(int flightId)
        {
            ViewBag.FlightId = flightId;
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FirstName,LastName,Email")] Passenger passenger, int FlightId)
        {
            if (ModelState.IsValid)
            {
                // Save passenger details to the database
                _context.Passengers.Add(passenger);
                _context.SaveChanges();
                return RedirectToAction("CreateForm", "BookingFlight", new { FlightId = FlightId, PassengerId = passenger.PassengerId });
            }
            return View(passenger);
        }

        [HttpGet]
        public IActionResult CreateForHotel(int hotelId)
        {
            ViewBag.HotelId = hotelId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateForHotel([Bind("FirstName,LastName,Email")] Passenger passenger, int HotelId)
        {
            if (ModelState.IsValid)
            {
                // Save passenger details to the database
                _context.Passengers.Add(passenger);
                _context.SaveChanges();
                return RedirectToAction("CreateForm", "BookingHotel", new { hotelId = HotelId, passengerId = passenger.PassengerId });
            }
            return View(passenger);
        }

        [HttpGet]
        public IActionResult CreateForCar(int carId)
        {
            ViewBag.CarId = carId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateForCar([Bind("FirstName,LastName,Email")] Passenger passenger, int CarId)
        {
            if (ModelState.IsValid)
            {
                // Save passenger details to the database
                _context.Passengers.Add(passenger);
                _context.SaveChanges();
                return RedirectToAction("CreateForm", "BookingCar", new { CarId = CarId, PassengerId = passenger.PassengerId });
            }
            return View(passenger);
        }
    }


}
