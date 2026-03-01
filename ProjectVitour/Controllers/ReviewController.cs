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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReview(CreateReviewDto createReviewDto)
        {
            if (string.IsNullOrWhiteSpace(createReviewDto.TourId))
            {
                return RedirectToAction("TourList", "Tour");
            }

            if (string.IsNullOrWhiteSpace(createReviewDto.NameSurname) ||
                string.IsNullOrWhiteSpace(createReviewDto.Detail) ||
                createReviewDto.GuideRating < 1 || createReviewDto.GuideRating > 5 ||
                createReviewDto.AccommodationRating < 1 || createReviewDto.AccommodationRating > 5 ||
                createReviewDto.TransportRating < 1 || createReviewDto.TransportRating > 5 ||
                createReviewDto.ComfortRating < 1 || createReviewDto.ComfortRating > 5)
            {
                TempData["ReviewError"] = "Lütfen ad-soyad, yorum ve tüm puan alanlarını 1-5 arası olacak şekilde doldurun.";
                return Redirect($"/Tour/Details/{createReviewDto.TourId}#content-reviews");
            }

            createReviewDto.NameSurname = createReviewDto.NameSurname.Trim();
            createReviewDto.Detail = createReviewDto.Detail.Trim();
            createReviewDto.Score = (int)Math.Round(
                (createReviewDto.GuideRating +
                 createReviewDto.AccommodationRating +
                 createReviewDto.TransportRating +
                 createReviewDto.ComfortRating) / 4.0
            );
            createReviewDto.ReviewDate = DateTime.Now;
            createReviewDto.Status = false;

            await _reviewService.CreateReviewAsync(createReviewDto);

            TempData["ReviewSuccess"] = "Yorumunuz alınmıştır. Onaylandıktan sonra yayınlanacaktır.";
            return Redirect($"/Tour/Details/{createReviewDto.TourId}#content-reviews");
        }
        public async Task<IActionResult> GetReviewByTourId(string id)
        {
            var values = await _reviewService.GetAllReviewsByTourIdAsync(id);
            return View(values);
        }

    }
}
