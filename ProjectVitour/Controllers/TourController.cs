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

        // Mevcut CreateTour ve TourList metotların burada duruyor...
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

        // --- YENİ EKLENECEK METOT ---
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            // Eğer id boş gelirse listeye geri yolla
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("TourList");
            }

            // Servis üzerinden id'ye ait tur bilgilerini getir
            var values = await _tourService.GetTourByIdAsync(id);

            // Eğer veritabanında böyle bir tur bulunamazsa 404 sayfasına yönlendir
            if (values == null)
            {
                return NotFound();
            }

            // Veriyi bulursak az önce hazırladığımız Details.cshtml sayfasına gönder
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
                // Hata mesajını frontend'e dön
                return StatusCode(500, new { success = false, message = "Bütçe hesaplanırken bir hata oluştu.", error = ex.Message });
            }
        }
    }
}