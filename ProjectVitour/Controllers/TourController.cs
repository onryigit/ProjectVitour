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
       
    }
    
}