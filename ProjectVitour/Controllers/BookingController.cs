using Microsoft.AspNetCore.Mvc;
using ProjectVitour.Dtos.ReservationDtos;
using ProjectVitour.Services.ReservationServices;
using ProjectVitour.Services.TourServices;
using ProjectVitour.Services.EmailServices;
using ProjectVitour.Helpers;
using ProjectVitour.Dtos.TourDtos;

namespace ProjectVitour.Controllers
{
    public class BookingController : Controller
    {
        private readonly ITourService _tourService;
        private readonly IReservationService _reservationService;
        private readonly IEmailService _emailService;

        public BookingController(ITourService tourService, IReservationService reservationService, IEmailService emailService)
        {
            _tourService = tourService;
            _reservationService = reservationService;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string tourId)
        {
            if (string.IsNullOrEmpty(tourId)) return RedirectToAction("Index", "Home");

            var tour = await _tourService.GetTourByIdAsync(tourId);
            if (tour == null) return NotFound();

            var localizedTitle = LocalizationHelper.GetLocalizedText(tour.Title, tour.Title_EN, tour.Title_DE);
            var localizedPrice = LocalizationHelper.GetLocalizedPrice(tour.Price);

            ViewBag.TourName = localizedTitle;
            ViewBag.TourPrice = localizedPrice;
            ViewBag.MaxCapacity = tour.Capacity;
            ViewBag.TourId = tourId;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateReservationDto createReservationDto)
        {
            // 1. Mevcut Tur bilgisini çek
            var tour = await _tourService.GetTourByIdAsync(createReservationDto.TourID);
            
            var localizedTitle = LocalizationHelper.GetLocalizedText(tour.Title, tour.Title_EN, tour.Title_DE);
            var localizedPrice = LocalizationHelper.GetLocalizedPrice(tour.Price);

            // 2. KONTENJAN KONTROLÜ
            if (createReservationDto.PersonCount > tour.Capacity)
            {
                TempData["ErrorMessage"] = $"Üzgünüz, bu turda sadece {tour.Capacity} kişilik yer kaldı.";
                
                ViewBag.TourName = localizedTitle;
                ViewBag.TourPrice = localizedPrice;
                ViewBag.MaxCapacity = tour.Capacity;
                ViewBag.TourId = createReservationDto.TourID;
                return View(createReservationDto);
            }

            // 3. Rezervasyonu Kaydet
            createReservationDto.ReservationDate = DateTime.Now;
            createReservationDto.Status = true; // Varsayılan onaylı
            await _reservationService.CreateReservationAsync(createReservationDto);

            // 4. Kapasiteyi Güncelle
            var updateDto = new UpdateTourDto
            {
                TourID = tour.TourID,
                Title = tour.Title,
                Title_EN = tour.Title_EN,
                Title_DE = tour.Title_DE,
                Description = tour.Description,
                Description_EN = tour.Description_EN,
                Description_DE = tour.Description_DE,
                CoverImageUrl = tour.CoverImageUrl,
                Badge = tour.Badge,
                DayCount = tour.DayCount,
                Capacity = tour.Capacity - createReservationDto.PersonCount,
                Price = tour.Price,
                Location = tour.Location,
                DestinationID = tour.DestinationID,
                MapLocationImageUrl = tour.MapLocationImageUrl,
                IsStatus = tour.IsStatus
            };
            await _tourService.UpdateTourAsync(updateDto);

            // 5. PROFESYONEL MAİL GÖNDERİMİ (Arka planda çalışır, kullanıcıyı bekletmez)
            string totalPrice = LocalizationHelper.GetLocalizedPrice(tour.Price * createReservationDto.PersonCount);
            string formattedDate = createReservationDto.ReservationDate.ToString("dd.MM.yyyy HH:mm");
            
            string reservationCode = "#VIT-" + Guid.NewGuid().ToString("N").Substring(0, 5).ToUpper();
            TempData["ReservationCode"] = reservationCode;

            _ = _emailService.SendReservationSuccessEmailAsync(
                createReservationDto.Email,
                createReservationDto.FullName,
                localizedTitle,
                formattedDate,
                totalPrice,
                createReservationDto.PersonCount,
                reservationCode
            );

            // 6. Başarılı sayfasına yönlendir
            return RedirectToAction("Success");
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }
    }
}