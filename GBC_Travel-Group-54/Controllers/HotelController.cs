using GBC_Travel_Group_54.Data;
using GBC_Travel_Group_54.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GBC_Travel_Group_54.Controllers
{
    public class HotelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HotelController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Hotel
        public IActionResult Index(string searchKeyword, int? numGuests, string sortOrder)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["LocationSortParm"] = sortOrder == "Location" ? "location_desc" : "Location";
            ViewData["PricingSortParm"] = sortOrder == "Pricing" ? "pricing_desc" : "Pricing";
            ViewData["RatingSortParm"] = sortOrder == "Rating" ? "rating_desc" : "Rating";

            var hotels = from h in _context.Hotels
                         select h;

            if (!String.IsNullOrEmpty(searchKeyword))
            {
                hotels = hotels.Where(h => h.Name.Contains(searchKeyword) || h.Location.Contains(searchKeyword));
            }

            if (numGuests.HasValue)
            {
                hotels = hotels.Where(h => h.NumGuest >= numGuests);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    hotels = hotels.OrderByDescending(h => h.Name);
                    break;
                case "Location":
                    hotels = hotels.OrderBy(h => h.Location);
                    break;
                case "location_desc":
                    hotels = hotels.OrderByDescending(h => h.Location);
                    break;
                case "Pricing":
                    hotels = hotels.OrderBy(h => h.PricingPerNight);
                    break;
                case "pricing_desc":
                    hotels = hotels.OrderByDescending(h => h.PricingPerNight);
                    break;
                case "Rating":
                    hotels = hotels.OrderBy(h => h.Rating);
                    break;
                case "rating_desc":
                    hotels = hotels.OrderByDescending(h => h.Rating);
                    break;
                default:
                    hotels = hotels.OrderBy(h => h.Name);
                    break;
            }

            return View(hotels.ToList());
        }

        [HttpGet]
        // GET: Hotel/ViewAllHotels
        public IActionResult ViewAllHotels(string sortOrder, string searchKeyword, int? numGuests)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["LocationSortParm"] = sortOrder == "Location" ? "location_desc" : "Location";
            ViewData["PricingSortParm"] = sortOrder == "Pricing" ? "pricing_desc" : "Pricing";
            ViewData["RatingSortParm"] = sortOrder == "Rating" ? "rating_desc" : "Rating";

            var hotels = from h in _context.Hotels
                         select h;

            if (!String.IsNullOrEmpty(searchKeyword))
            {
                hotels = hotels.Where(h => h.Location.Contains(searchKeyword));
            }

            if (numGuests.HasValue)
            {
                hotels = hotels.Where(h => h.NumGuest >= numGuests);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    hotels = hotels.OrderByDescending(h => h.Name);
                    break;
                case "Location":
                    hotels = hotels.OrderBy(h => h.Location);
                    break;
                case "location_desc":
                    hotels = hotels.OrderByDescending(h => h.Location);
                    break;
                case "Pricing":
                    hotels = hotels.OrderBy(h => h.PricingPerNight);
                    break;
                case "pricing_desc":
                    hotels = hotels.OrderByDescending(h => h.PricingPerNight);
                    break;
                case "Rating":
                    hotels = hotels.OrderBy(h => h.Rating);
                    break;
                case "rating_desc":
                    hotels = hotels.OrderByDescending(h => h.Rating);
                    break;
                default:
                    hotels = hotels.OrderBy(h => h.Name);
                    break;
            }

            return View(hotels.ToList());
        }



        public IActionResult ViewHotelAmenities(int hotelId)
        {
            var amenities = _context.HotelAmenities
                .Include(ha => ha.Amenity)
                .Where(ha => ha.HotelId == hotelId)
                .Select(ha => ha.Amenity)
                .ToList();

            return View(amenities);
        }


        // GET: Hotel/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = _context.Hotels.FirstOrDefault(m => m.HotelId == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }
        // GET: Hotel/Create
        public IActionResult Create()
        {
            var predefinedAmenities = _context.Amenities.ToList();
            ViewBag.PredefinedAmenities = new MultiSelectList(predefinedAmenities, "AmenityId", "AmenityName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,NumGuest,Location,PricingPerNight,Rating")] Hotel hotel, List<int> selectedAmenities)
        {
            if (ModelState.IsValid)
            {
                _context.Hotels.Add(hotel);
                _context.SaveChanges();

                if (selectedAmenities != null)
                {
                    foreach (var amenityId in selectedAmenities)
                    {
                        var selectedAmenity = _context.Amenities.Find(amenityId);
                        if (selectedAmenity != null)
                        {
                            _context.HotelAmenities.Add(new HotelAmenities { HotelId = hotel.HotelId, AmenityId = amenityId });
                        }
                    }
                    _context.SaveChanges();
                }

                return RedirectToAction("ViewAllHotels");
            }

            ViewBag.PredefinedAmenities = new MultiSelectList(_context.Amenities.ToList(), "AmenityId", "AmenityName");
            return View(hotel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }


            var predefinedAmenities = _context.Amenities.ToList();
            ViewBag.PredefinedAmenities = new MultiSelectList(predefinedAmenities, "AmenityId", "AmenityName");
            return View(hotel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HotelId,Name,NumGuest,Location,PricingPerNight,Rating")] Hotel hotel, List<int> selectedAmenities)
        {
            if (id != hotel.HotelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hotel);
                    await _context.SaveChangesAsync();

                    // Update amenities
                    var existingAmenities = _context.HotelAmenities.Where(ha => ha.HotelId == hotel.HotelId);
                    _context.HotelAmenities.RemoveRange(existingAmenities);

                    foreach (var amenityId in selectedAmenities)
                    {
                        _context.HotelAmenities.Add(new HotelAmenities { HotelId = hotel.HotelId, AmenityId = amenityId });
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelExists(hotel.HotelId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ViewAllHotels");
            }
            return RedirectToAction("ViewAllHotels");
        }

        private bool HotelExists(int id)
        {
            return _context.Hotels.Any(e => e.HotelId == id);
        }


        // GET: Hotel/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = _context.Hotels.FirstOrDefault(m => m.HotelId == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int HotelId)
        {
            var hotel = _context.Hotels.Find(HotelId);
            if (hotel != null)
            {
                _context.Hotels.Remove(hotel);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult SearchHotels(string searchKeyword, int? numGuests, decimal? minPrice, decimal? maxPrice)
        {
            var hotelsQuery = _context.Hotels.AsQueryable();

            if (!string.IsNullOrEmpty(searchKeyword))
            {
                hotelsQuery = hotelsQuery.Where(h => h.Name.Contains(searchKeyword) || h.Location.Contains(searchKeyword));
            }

            if (numGuests.HasValue)
            {
                hotelsQuery = hotelsQuery.Where(h => h.NumGuest >= numGuests.Value);
            }

            if (minPrice.HasValue)
            {
                hotelsQuery = hotelsQuery.Where(h => h.PricingPerNight >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                hotelsQuery = hotelsQuery.Where(h => h.PricingPerNight <= maxPrice.Value);
            }

            // Ensure this view name correctly matches your View file for displaying search results.
            // It might be "Index" or another view if you're displaying search results differently.
            var filteredHotels = hotelsQuery.ToList();
            return View("UserHotel", filteredHotels); // Or replace "Index" with your specific View name for search results.
        }




        public IActionResult BookHotel(int hotelId)
        {
            var hotel = _context.Hotels.Find(hotelId);

            if (hotel == null)
            {
                // Handle the case where the car with the specified ID is not found
                return NotFound();
            }

            // Redirect to the Passenger controller's Create action with car details
            return RedirectToAction("CreateForHotel", "Passenger", new { hotelId = hotelId});
        }
    }
}
