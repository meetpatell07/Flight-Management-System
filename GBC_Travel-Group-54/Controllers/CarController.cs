    using GBC_Travel_Group_54.Data;
    using GBC_Travel_Group_54.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;

    namespace GBC_Travel_Group_54.Controllers
    {
        public class CarController : Controller
        {
            private readonly ApplicationDbContext _context;

            public CarController(ApplicationDbContext context)
            {
                _context = context;
            }

            [HttpGet]
            public IActionResult Index()
            {
                return View();
            }

            [HttpGet]
            public IActionResult Details(int id)
            {
                var car = _context.Cars.FirstOrDefault(c => c.CarId == id);
                if (car == null)
                {
                    return NotFound();
                }
                return View(car);
            }

            [HttpGet]
            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Create([Bind("Make,Model,NumPassenger,Year,RentalCompanies,RentalPricePerDay, NumPassenger")] Car car)
            {
                if (ModelState.IsValid)
                {
                    car.IsAvailable = true; // Assuming the car is available when it's created
                    _context.Cars.Add(car);
                    _context.SaveChanges();
                    return RedirectToAction("ViewAllCars");
                }
                return View(car); // Return the view with validation errors
            }

            [HttpGet]
            public IActionResult Edit(int id)
            {
                var car = _context.Cars.Find(id);
                if (car == null)
                {
                    return NotFound();
                }
                return View(car);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Edit(int id, [Bind("CarId,Make,Model,NumPassenger,Year,RentalCompanies,RentalPricePerDay,IsAvailable")] Car car)
            {
                if (id != car.CarId)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(car);
                        _context.SaveChanges();
                        return RedirectToAction("ViewAllCars");
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CarExists(car.CarId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                return RedirectToAction("ViewAllCars");
            }

            private bool CarExists(int carId)
            {
                return _context.Cars.Any(car => car.CarId == carId);
            }

            [HttpGet]
            public IActionResult Delete(int id)
            {
                var car = _context.Cars.FirstOrDefault(c => c.CarId == id);
                if (car == null)
                {
                    return NotFound();

                }
                return View(car);
            }

            [HttpPost, ActionName("DeleteConfirmed")]
            [ValidateAntiForgeryToken]
            public IActionResult DeleteConfirmed(int CarId)
            {
                var car = _context.Cars.Find(CarId);
                if (car != null)
                {
                    _context.Cars.Remove(car);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                return NotFound();
            }

            [HttpGet]
            [HttpGet]
            public IActionResult ViewAllCars(string sortOrder, int? passengers, decimal? minPrice, decimal? maxPrice, bool? isAvailable)
            {
                ViewData["PriceSortParm"] = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
                ViewData["YearSortParm"] = sortOrder == "Year" ? "year_desc" : "Year";
                ViewData["PassengerSortParm"] = sortOrder == "Passenger" ? "passenger_desc" : "Passenger";
                ViewData["AvailableSortParm"] = sortOrder == "Available" ? "available_desc" : "Available";

                var cars = from s in _context.Cars select s;

                // Apply filters
                if (passengers.HasValue)
                {
                    cars = cars.Where(c => c.NumPassenger >= passengers);
                }
                if (minPrice.HasValue)
                {
                    cars = cars.Where(c => c.RentalPricePerDay >= minPrice);
                }
                if (maxPrice.HasValue)
                {
                    cars = cars.Where(c => c.RentalPricePerDay <= maxPrice);
                }
                if (isAvailable.HasValue)
                {
                    cars = cars.Where(c => c.IsAvailable == isAvailable);
                }

                // Apply sorting
                switch (sortOrder)
                {
                    case "price_desc":
                        cars = cars.OrderByDescending(s => s.RentalPricePerDay);
                        break;
                    case "Year":
                        cars = cars.OrderBy(s => s.Year);
                        break;
                    case "year_desc":
                        cars = cars.OrderByDescending(s => s.Year);
                        break;
                    case "Passenger":
                        cars = cars.OrderBy(s => s.NumPassenger);
                        break;
                    case "passenger_desc":
                        cars = cars.OrderByDescending(s => s.NumPassenger);
                        break;
                    case "Available":
                        cars = cars.OrderBy(s => s.IsAvailable);
                        break;
                    case "available_desc":
                        cars = cars.OrderByDescending(s => s.IsAvailable);
                        break;
                    default:
                        cars = cars.OrderBy(s => s.RentalPricePerDay);
                        break;
                }

                return View(cars.ToList());
            }


            public IActionResult BookCar(int carId)
            {
                var car = _context.Cars.Find(carId);

                if (car == null)
                {
                    // Handle the case where the car with the specified ID is not found
                    return NotFound();
                }

                // Redirect to the Passenger controller's Create action with car details
                return RedirectToAction("CreateForCar", "Passenger", new { carId = carId });
            }


            [HttpPost]
            public IActionResult SearchCars(int passengers, decimal minPrice, decimal maxPrice)
            {
                // Filter cars based on the number of passengers and price range
                var availableCars = _context.Cars
                    .Where(c => c.NumPassenger >= passengers && c.RentalPricePerDay >= minPrice && c.RentalPricePerDay <= maxPrice)
                    .ToList();

                // Pass the filtered list of available cars to the view
                return View("Index", availableCars);
            }



        }
    }