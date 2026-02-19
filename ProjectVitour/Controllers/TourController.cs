using Microsoft.AspNetCore.Mvc;
using ProjectVitour.Dtos.TourDtos;
using ProjectVitour.Services.TourServices;

namespace ProjectVitour.Controllers
{
    public class TourController : Controller
    {
        private readonly ITourService _tourService;

        public TourController(ITourService tourService)
        {
            _tourService = tourService;
        }

        public IActionResult CreateTour()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>CreateTour(CreateTourDto createTourDto)
        {
            await _tourService.CreateTourAsync(createTourDto);
            return RedirectToAction("TourList");
        }
        public IActionResult TourList()
        {
            return View();
        }
    }
}
