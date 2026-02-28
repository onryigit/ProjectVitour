using Microsoft.AspNetCore.Mvc;
using ProjectVitour.Dtos.ReviewDtos;
using ProjectVitour.Services.ReviewServices;

namespace ProjectVitour.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public IActionResult CreateReview(string tourId)
        {
            var model = new CreateReviewDto
            {
                TourId = tourId,
                ReviewDate = DateTime.Now // Tarihi backend'den bugüne ayarladık!
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateReview(CreateReviewDto createReviewDto)
        {
            // Yorumu onaysız olarak kaydet (İstersen test aşamasındayken burayı true yapabilirsin)
            createReviewDto.Status = false;

            await _reviewService.CreateReviewAsync(createReviewDto);

            // HATA VEREN ESKİ KOD: return RedirectToAction("ReviewList");
            // YENİ KOD: Yorum yapılan turun detay sayfasına (Tour/Details/ID) geri dön
            return RedirectToAction("Details", "Tour", new { id = createReviewDto.TourId });
        }
        public async Task<IActionResult> GetReviewByTourId(string id)
        {
            var values = await _reviewService.GetAllReviewsByTourIdAsync(id);
            return View(values);
        }

    }
}
