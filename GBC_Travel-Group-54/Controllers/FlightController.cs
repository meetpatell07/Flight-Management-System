using GBC_Travel_Group_54.Data;
using GBC_Travel_Group_54.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace GBC_Travel_Group_54.Controllers
{
    public class FlightController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlightController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string departureAirport, string arrivalAirport, DateTime? departureDate, decimal? minPrice, decimal? maxPrice, string sortOrder)
        {
            var flightsQuery = _context.Flights.AsQueryable();

            // Only perform the query if at least one search parameter is provided
            if (!string.IsNullOrWhiteSpace(departureAirport) || !string.IsNullOrWhiteSpace(arrivalAirport) || departureDate.HasValue || minPrice.HasValue || maxPrice.HasValue)
            {
                // Apply filters based on the provided search parameters
                if (!string.IsNullOrWhiteSpace(departureAirport))
                {
                    flightsQuery = flightsQuery.Where(f => f.DepartureAirport.Contains(departureAirport));
                }

                if (!string.IsNullOrWhiteSpace(arrivalAirport))
                {
                    flightsQuery = flightsQuery.Where(f => f.ArrivalAirport.Contains(arrivalAirport));
                }

                if (departureDate.HasValue)
                {
                    flightsQuery = flightsQuery.Where(f => f.DepartureDateTime.Date == departureDate.Value.Date);
                }

                if (minPrice.HasValue)
                {
                    flightsQuery = flightsQuery.Where(f => f.Price >= minPrice.Value);
                }

                if (maxPrice.HasValue)
                {
                    flightsQuery = flightsQuery.Where(f => f.Price <= maxPrice.Value);
                }

                // Apply sorting based on sortOrder
                // Implement sorting logic here based on sortOrder parameter

                var flights = await flightsQuery.ToListAsync();
                return View(flights);
            }

            // Return an empty list or null to the view if no search parameters were provided
            return View(new List<Flight>());
        }



        [HttpGet]
        public IActionResult ViewAllFlights(string sortOrder)
        {
            ViewData["PriceSortParm"] = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewData["DepartureSortParm"] = sortOrder == "Departure" ? "departure_desc" : "Departure";
            ViewData["ArrivalSortParm"] = sortOrder == "Arrival" ? "arrival_desc" : "Arrival";

            var flights = from f in _context.Flights
                          select f;

            switch (sortOrder)
            {
                case "price_desc":
                    flights = flights.OrderByDescending(f => f.Price);
                    break;
                case "Departure":
                    flights = flights.OrderBy(f => f.DepartureDateTime.Date);
                    break;
                case "departure_desc":
                    flights = flights.OrderByDescending(f => f.DepartureDateTime.Date);
                    break;
                case "Arrival":
                    flights = flights.OrderBy(f => f.ArrivalDateTime.Date);
                    break;
                case "arrival_desc":
                    flights = flights.OrderByDescending(f => f.ArrivalDateTime.Date);
                    break;
                default:
                    flights = flights.OrderBy(f => f.Price);
                    break;
            }

            return View(flights.ToList());
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var flight = _context.Flights.FirstOrDefault(f => f.FlightId == id);
            if (flight == null)
            {
                return NotFound();
            }
            return View(flight);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("DepartureAirport,ArrivalAirport,Airline,DepartureDateTime,ArrivalDateTime,Price")] Flight flight)
        {
            if (ModelState.IsValid)
            {
                _context.Flights.Add(flight);
                _context.SaveChanges();
                ViewBag.FlightAdded = true;
                return RedirectToAction("ViewAllFlights");
            }
            return View(flight); // Return the view with validation errors
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var flight = _context.Flights.Find(id);
            if (flight == null)
            {
                return NotFound();
            }
            return View(flight);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("FlightId,DepartureAirport,ArrivalAirport,Airline,DepartureDateTime,ArrivalDateTime,Price")] Flight flight)
        {
            if (id != flight.FlightId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Update(flight);
                _context.SaveChanges();
                return RedirectToAction("ViewAllFlights");
            }
            return View(flight);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var flight = _context.Flights.FirstOrDefault(f => f.FlightId == id);
            if (flight == null)
            {
                return NotFound();
            }
            return View(flight);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int FlightId)
        {
            var flight = _context.Flights.Find(FlightId);
            if (flight != null)
            {
                _context.Flights.Remove(flight);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult SearchFlights(string departureAirport, string arrivalAirport, DateTime departureDate, decimal? minPrice, decimal? maxPrice)
        {
            var flights = _context.Flights
                .Where(f => f.DepartureAirport.Contains(departureAirport) &&
                            f.ArrivalAirport.Contains(arrivalAirport) &&
                            f.DepartureDateTime.Date >= departureDate.Date);

            // Filter by minimum price if specified
            if (minPrice.HasValue)
            {
                flights = flights.Where(f => f.Price >= minPrice.Value);
            }

            // Filter by maximum price if specified
            if (maxPrice.HasValue)
            {
                flights = flights.Where(f => f.Price <= maxPrice.Value);
            }

            return View("ViewAllFlights", flights.ToList());
        }

        [HttpGet]
        public IActionResult BookFlight(int flightId)
        {
            // Pass the flight ID to the Passenger Create view
            var flight = _context.Flights.Find(flightId);

            // Redirect to the Passenger controller's Create action with flight details
            return RedirectToAction("Create", "Passenger", new { flightId });
        }
    }
}