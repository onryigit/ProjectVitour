using Microsoft.AspNetCore.Mvc;
using ProjectVitour.Dtos.TourDtos;
using ProjectVitour.Services.GeminiServices;
using ProjectVitour.Services.TourServices;

namespace ProjectVitour.Controllers
{
    public class TourController : Controller
    {
        private readonly ITourService _tourService;
        private readonly IGeminiCostService _geminiCostService;

        public TourController(ITourService tourService, IGeminiCostService geminiCostService)
        {
            _tourService = tourService;
            _geminiCostService = geminiCostService;
        }
        public IActionResult CreateTour()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTour(CreateTourDto createTourDto)
        {
            await _tourService.CreateTourAsync(createTourDto);
            return RedirectToAction("TourList");
        }

        public IActionResult TourList()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("TourList");
            }
            var values = await _tourService.GetTourByIdAsync(id);
            if (values == null)
            {
                return NotFound();
            }
           return View (values);
        }
        
        [HttpPost]
        public async Task<IActionResult> CalculateHiddenCost(string location, int days, decimal tourPrice, string includedItems)
        {
            try
            {
                if (string.IsNullOrEmpty(location) || days <= 0 || tourPrice <= 0)
                {
                    return BadRequest(new { success = false, message = "Geçersiz parametreler." });
                }

                var costData = await _geminiCostService.CalculateHiddenCostAsync(location, days, tourPrice, includedItems ?? "");
                return Json(new { success = true, data = costData });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Bütçe hesaplanırken bir hata oluştu.", error = ex.Message });
            }
        }
    }
}