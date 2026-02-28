using Microsoft.AspNetCore.Mvc;
using ProjectVitour.Dtos.ReservationDtos;
using ProjectVitour.Services.ReservationServices;
using ProjectVitour.Services.TourServices;

namespace ProjectVitour.Controllers
{
    public class BookingController : Controller
    {
        private readonly ITourService _tourService;
        private readonly IReservationService _reservationService;

        public BookingController(ITourService tourService, IReservationService reservationService)
        {
            _tourService = tourService;
            _reservationService = reservationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string tourId)
        {
            if (string.IsNullOrEmpty(tourId)) return RedirectToAction("Index", "Home");

            var tour = await _tourService.GetTourByIdAsync(tourId);
            if (tour == null) return NotFound();

            ViewBag.TourName = tour.Title;
            ViewBag.TourPrice = tour.Price;
            ViewBag.MaxCapacity = tour.Capacity;
            ViewBag.TourId = tourId;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateReservationDto createReservationDto)
        {
            // 1. Mevcut Tur bilgisini çek
            var tour = await _tourService.GetTourByIdAsync(createReservationDto.TourID);
            
            // 2. KONTENJAN KONTROLÜ (Case 3 Madde 1.3)
            if (createReservationDto.PersonCount > tour.Capacity)
            {
                TempData["ErrorMessage"] = $"Üzgünüz, bu turda sadece {tour.Capacity} kişilik yer kaldı.";
                
                ViewBag.TourName = tour.Title;
                ViewBag.TourPrice = tour.Price;
                ViewBag.MaxCapacity = tour.Capacity;
                ViewBag.TourId = createReservationDto.TourID;
                return View(createReservationDto);
            }

            // 3. Rezervasyonu Kaydet
            createReservationDto.ReservationDate = DateTime.Now;
            createReservationDto.Status = true; // Varsayılan onaylı
            await _reservationService.CreateReservationAsync(createReservationDto);

            // 4. Kapasiteyi Güncelle (Opsiyonel ama mantıklı)
            tour.Capacity -= createReservationDto.PersonCount;
            // UpdateTourDto'ya çevirip güncelleme yapabiliriz
            // Not: TourService'de UpdateTourAsync metodumuz var.

            TempData["SuccessMessage"] = "Rezervasyonunuz başarıyla alındı! Sizinle en kısa sürede iletişime geçeceğiz.";
            return RedirectToAction("Index", "Home");
        }
    }
}