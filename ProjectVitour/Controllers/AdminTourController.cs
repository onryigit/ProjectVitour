using Microsoft.AspNetCore.Mvc;
using ProjectVitour.Dtos.TourDtos;
using ProjectVitour.Services.TourServices;
using ProjectVitour.Services.DestinationServices;

namespace Project3ViTour.Controllers
{
    public class AdminTourController : Controller
    {
        private readonly ITourService _tourService;
        private readonly IDestinationService _destinationService;

        public AdminTourController(ITourService tourService, IDestinationService destinationService)
        {
            _tourService = tourService;
            _destinationService = destinationService;
        }

        public async Task<IActionResult> TourList()
        {
            var values = await _tourService.GetAllToursAsync();
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> CreateTour()
        {
            ViewBag.Destinations = await _destinationService.GetAllDestinationsAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTour(CreateTourDto createDtoTour)
        {
            await _tourService.CreateTourAsync(createDtoTour);
            return RedirectToAction("TourList");
        }
        
        public async Task<IActionResult> DeleteTour(string id)
        {
            await _tourService.DeleteTourAsync(id);
            return RedirectToAction("TourList");
        }

        // HIZLI DURUM GÜNCELLEME (TOGGLE)
        public async Task<IActionResult> ToggleTourStatus(string id)
        {
            var tour = await _tourService.GetTourByIdAsync(id);
            if (tour != null)
            {
                var updateDto = new UpdateTourDto
                {
                    TourID = tour.TourID,
                    Title = tour.Title,
                    Description = tour.Description,
                    CoverImageUrl = tour.CoverImageUrl,
                    Badge = tour.Badge,
                    DayCount = tour.DayCount,
                    Capacity = tour.Capacity,
                    Price = tour.Price,
                    Location = tour.Location,
                    DestinationID = tour.DestinationID,
                    MapLocationImageUrl = tour.MapLocationImageUrl,
                    IsStatus = !tour.IsStatus // Durumu tersine çevir
                };
                await _tourService.UpdateTourAsync(updateDto);
            }
            return RedirectToAction("TourList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateTour(string id)
        {
            ViewBag.Destinations = await _destinationService.GetAllDestinationsAsync();
            var value = await _tourService.GetTourByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTour(UpdateTourDto updateTourDto)
        {
            await _tourService.UpdateTourAsync(updateTourDto);
            return RedirectToAction("TourList");
        }
    }
}