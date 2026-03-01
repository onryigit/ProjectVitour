using Microsoft.AspNetCore.Mvc;
using ProjectVitour.Dtos.TourDtos;
using ProjectVitour.Services.TourServices;
using ProjectVitour.Services.DestinationServices;
using ProjectVitour.Services.CategoryServices;
using System;
using System.Threading.Tasks;

namespace ProjectVitour.Controllers
{
    public class AdminTourController : Controller
    {
        private readonly ITourService _tourService;
        private readonly IDestinationService _destinationService;
        private readonly ICategoryService _categoryService;

        public AdminTourController(ITourService tourService, IDestinationService destinationService, ICategoryService categoryService)
        {
            _tourService = tourService;
            _destinationService = destinationService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> TourList()
        {
            var values = await _tourService.GetAllToursAsync();
            return View(values);
        }

        // --- CREATE (GET) ---
        [HttpGet]
        public async Task<IActionResult> CreateTour()
        {
            try 
            {
                ViewBag.Destinations = await _destinationService.GetAllDestinationsAsync();
                ViewBag.Categories = await _categoryService.GetAllCategoryAsync();
                return View(new CreateTourDto());
            }
            catch (Exception ex)
            {
                // Bir hata olursa en azından sayfa açılsın
                ModelState.AddModelError("", "Sayfa yüklenirken bir hata oluştu: " + ex.Message);
                return View(new CreateTourDto());
            }
        }

        // --- CREATE (POST) ---
        [HttpPost]
        public async Task<IActionResult> CreateTour(CreateTourDto createDtoTour)
        {
            try
            {
                // Destinasyon ID'sinden isim ve ülkeyi alıyoruz
                if (!string.IsNullOrEmpty(createDtoTour.DestinationID))
                {
                    var dest = await _destinationService.GetDestinationByIdAsync(createDtoTour.DestinationID);
                    if(dest != null)
                    {
                        createDtoTour.Location = $"{dest.CityName}, {dest.CountryName}";
                    }
                }
                
                // Formdan boş gelse bile hata vermemesi için varsayılan atamalar
                if (string.IsNullOrEmpty(createDtoTour.Location)) createDtoTour.Location = "Belirtilmemiş";
                if (string.IsNullOrEmpty(createDtoTour.MapLocationImageUrl)) createDtoTour.MapLocationImageUrl = "";
                
                createDtoTour.IsStatus = true; // Yeni eklenen tur varsayılan olarak aktif başlasın

                await _tourService.CreateTourAsync(createDtoTour);
                return RedirectToAction("TourList");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Tur kaydedilirken bir hata oluştu: " + ex.Message);
                
                // Hata durumunda dropdown'ların boş kalmaması için listeleri tekrar dolduruyoruz
                ViewBag.Destinations = await _destinationService.GetAllDestinationsAsync();
                ViewBag.Categories = await _categoryService.GetAllCategoryAsync();
                return View(createDtoTour);
            }
        }
        
        // --- DELETE ---
        public async Task<IActionResult> DeleteTour(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("TourList");
            
            try 
            {
                await _tourService.DeleteTourAsync(id);
            }
            catch (Exception)
            {
                // Opsiyonel: TempData ile hata mesajı dönülebilir
            }
            return RedirectToAction("TourList");
        }

        // --- TOGGLE STATUS ---
        public async Task<IActionResult> ToggleTourStatus(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("TourList");

            try 
            {
                var tour = await _tourService.GetTourByIdAsync(id);
                if (tour != null)
                {
                    var updateDto = new UpdateTourDto
                    {
                        TourID = tour.TourID,
                        Title = tour.Title ?? "",
                        Title_EN = tour.Title_EN ?? "",
                        Title_DE = tour.Title_DE ?? "",
                        Description = tour.Description ?? "",
                        Description_EN = tour.Description_EN ?? "",
                        Description_DE = tour.Description_DE ?? "",
                        CoverImageUrl = tour.CoverImageUrl ?? "",
                        Badge = tour.Badge ?? "",
                        DayCount = tour.DayCount,
                        Capacity = tour.Capacity,
                        Price = tour.Price,
                        Location = tour.Location ?? "",
                        DestinationID = tour.DestinationID,
                        CategoryID = tour.CategoryID,
                        MapLocationImageUrl = tour.MapLocationImageUrl ?? "",
                        IsStatus = !tour.IsStatus // Durumu tersine çevir
                    };
                    await _tourService.UpdateTourAsync(updateDto);
                }
            }
            catch (Exception)
            {
                // Opsiyonel: Loglama yapılabilir
            }
            return RedirectToAction("TourList");
        }

        // --- UPDATE (GET) ---
        [HttpGet]
        public async Task<IActionResult> UpdateTour(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("TourList");

            try
            {
                ViewBag.Destinations = await _destinationService.GetAllDestinationsAsync();
                ViewBag.Categories = await _categoryService.GetAllCategoryAsync();
                
                var value = await _tourService.GetTourByIdAsync(id);
                if (value == null) return RedirectToAction("TourList");
                
                return View(value);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Tur bilgileri getirilirken bir hata oluştu: " + ex.Message);
                return RedirectToAction("TourList");
            }
        }

        // --- UPDATE (POST) ---
        [HttpPost]
        public async Task<IActionResult> UpdateTour(UpdateTourDto updateTourDto)
        {
            try
            {
                // Destinasyon ID'sinden güncel lokasyon ismini çek
                if (!string.IsNullOrEmpty(updateTourDto.DestinationID))
                {
                    var dest = await _destinationService.GetDestinationByIdAsync(updateTourDto.DestinationID);
                    if(dest != null)
                    {
                        updateTourDto.Location = $"{dest.CityName}, {dest.CountryName}";
                    }
                }

                if (string.IsNullOrEmpty(updateTourDto.Location)) updateTourDto.Location = "Belirtilmemiş";
                if (string.IsNullOrEmpty(updateTourDto.MapLocationImageUrl)) updateTourDto.MapLocationImageUrl = "";

                await _tourService.UpdateTourAsync(updateTourDto);
                return RedirectToAction("TourList");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Güncelleme sırasında bir hata oluştu: " + ex.Message);
                
                // Form geri dönerken verilerin (Dropdown vb.) kopmaması için tekrar doldur
                ViewBag.Destinations = await _destinationService.GetAllDestinationsAsync();
                ViewBag.Categories = await _categoryService.GetAllCategoryAsync();
                
                // Model GetTourByIdDto beklediği için Update'i dönüştürüyoruz
                var returnDto = new GetTourByIdDto 
                {
                    TourID = updateTourDto.TourID,
                    Title = updateTourDto.Title,
                    Title_EN = updateTourDto.Title_EN,
                    Title_DE = updateTourDto.Title_DE,
                    Description = updateTourDto.Description,
                    Description_EN = updateTourDto.Description_EN,
                    Description_DE = updateTourDto.Description_DE,
                    CoverImageUrl = updateTourDto.CoverImageUrl,
                    Badge = updateTourDto.Badge,
                    DayCount = updateTourDto.DayCount,
                    Capacity = updateTourDto.Capacity,
                    Price = updateTourDto.Price,
                    Location = updateTourDto.Location,
                    DestinationID = updateTourDto.DestinationID,
                    CategoryID = updateTourDto.CategoryID,
                    MapLocationImageUrl = updateTourDto.MapLocationImageUrl,
                    IsStatus = updateTourDto.IsStatus
                };
                return View(returnDto);
            }
        }
    }
}