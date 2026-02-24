using Microsoft.AspNetCore.Mvc;
using Project3ViTour.Dtos.TourDtos;
using Project3ViTour.Services.TourService;

namespace Project3ViTour.Controllers
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
        public async Task<IActionResult> CreateTour(CreateTourDto createDtoTour)
        {
            await _tourService.CreateTourAsync(createDtoTour);
            return RedirectToAction("TourList");
        }

        public IActionResult TourList()
        {
            return View();
        }

    }

}
